
using MQTTnet;
using MQTTnet.Client;

namespace SmartTraficControlSystem.Components.Services
{
    public class MqttService
    {
        private IMqttClient _mqttClient;
        private bool _vehicleLightIsGreen = false;
        private string _lastPayload = string.Empty;

        // Event that gets raised when a message is received
        public event Action<bool>? OnVehicleLightChanged;
        public event Action<string>? OnMessageReceived;

        public async Task ConnectAsync()
        {
            var factory = new MQTTnet.MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MQTTnet.Client.Options.MqttClientOptionsBuilder()
                .WithClientId("SmartTrafficControlSystemClient")
                .WithTcpServer("localhost", 1883) //todo make docker compatible
                .WithCleanSession()
                .Build();

            // Handle incoming messages
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = e.ApplicationMessage.ConvertPayloadToString();
                _lastPayload = payload;

                // Parse boolean from MQTT message
                bool isGreen = payload == "VEHICLE_GREEN";
                _vehicleLightIsGreen = isGreen;

                // Notify subscribers
                OnVehicleLightChanged?.Invoke(isGreen);
                OnMessageReceived?.Invoke(payload);

                return Task.CompletedTask;
            });

            try
            {
                await _mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT connection failed: {ex.Message}");
            }
        }

        public async Task SubscribeAsync(string topic = "traffic/light")
        {
            if (_mqttClient?.IsConnected == true)
            {
                await _mqttClient.SubscribeAsync(topic);
            }
        }
    }
}