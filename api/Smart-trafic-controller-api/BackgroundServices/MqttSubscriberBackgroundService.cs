using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.BackgroundServices
{
    public class MqttSubscriberBackgroundService : BackgroundService
    {
        private readonly ILogger<MqttSubscriberBackgroundService> _logger;
        private readonly ISensorLogService _sensorLogService;

        private IMqttClient _mqttClient;

        public MqttSubscriberBackgroundService(ILogger<MqttSubscriberBackgroundService> logger, ISensorLogService sensorLogService)
        {
            _logger = logger;
            _sensorLogService = sensorLogService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) 
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
                _logger.LogWarning("Disconnected from MQTT broker.");
            });

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation($"Received message on topic {e.ApplicationMessage.Topic}: {payload}");

                // TODO: Finish this implementation
                // _sensorLogService.CreateSensorLogAsync();
            });

            await _mqttClient.ConnectAsync(options, stoppingToken);
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