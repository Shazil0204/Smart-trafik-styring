using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Smart_traffic_controller_api.Enums;
using Smart_traffic_controller_api.Interfaces;

namespace Smart_traffic_controller_api.BackgroundServices
{
    public class MqttSubscriberBackgroundService : BackgroundService
    {
        private readonly ILogger<MqttSubscriberBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private IMqttClient _mqttClient;

        public MqttSubscriberBackgroundService(
            ILogger<MqttSubscriberBackgroundService> logger,
            IServiceScopeFactory scopeFactory
        )
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("mosquitto", 1883)
                .WithClientId("aspnet-subscriber")
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("Connected to MQTT broker.");

                // Subscribe to topic(s)
                await _mqttClient.SubscribeAsync("traffic/light");
                _logger.LogInformation("Subscribed to topic: traffic/light");
            });

            _mqttClient.UseDisconnectedHandler(e =>
            {
                // Some MqttNet versions expose only Exception on disconnect events.
                _logger.LogWarning(
                    "Disconnected from MQTT broker. Exception: {exception}",
                    e?.Exception?.Message
                );
            });

            _mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                try
                {
                    var payload = Encoding.UTF8.GetString(
                        e.ApplicationMessage.Payload ?? Array.Empty<byte>()
                    );
                    _logger.LogInformation(
                        "Received message on topic {topic}: {payload}",
                        e.ApplicationMessage?.Topic,
                        payload
                    );

                    if (!Enum.TryParse<SensorValue>(payload, out var sensorValue))
                    {
                        _logger.LogError("Invalid sensor value received: {payload}", payload);
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var sensorLogService =
                        scope.ServiceProvider.GetRequiredService<ISensorLogService>();

                    // Await the async call while the scope is still alive to avoid using a disposed DbContext
                    try
                    {
                        await sensorLogService.CreateSensorLogAsync(sensorValue);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create sensor log from MQTT message.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception while processing MQTT message.");
                }
            });

            // Retry/connect loop with exponential backoff. This keeps attempting to connect
            // until the service is stopped. We avoid starting multiple concurrent connect
            // attempts by only connecting from this loop and checking IsConnected.
            var backoffSeconds = 1;
            const int maxBackoffSeconds = 30;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_mqttClient.IsConnected)
                {
                    // Already connected; wait a bit before re-checking
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    continue;
                }

                try
                {
                    _logger.LogInformation("Attempting to connect to MQTT broker...");
                    await _mqttClient.ConnectAsync(options, stoppingToken);
                    // Reset backoff after successful connection
                    backoffSeconds = 1;
                }
                catch (OperationCanceledException)
                {
                    // Stopping token triggered
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to connect to MQTT broker. Retrying in {seconds} seconds.",
                        backoffSeconds
                    );
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(backoffSeconds), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    // Exponential backoff with a cap
                    backoffSeconds = Math.Min(backoffSeconds * 2, maxBackoffSeconds);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();
            }
            await base.StopAsync(cancellationToken);
        }
    }
}
