using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Entities
{
    public class SensorLog
    {
        public int Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public SensorValue SensorValue { get; private set; }

        private SensorLog() { } // For EF Core

        public SensorLog(SensorValue sensorValue)
        {
            Timestamp = DateTime.UtcNow;
            SensorValue = sensorValue;
        }
    }
}