using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Entities
{
    public class SensorLog
    {
        public int Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string SensorType { get; private set; } = null!;
        public string SensorValue { get; private set; } = null!;

        private SensorLog() { } // For EF Core

        public SensorLog(string sensorType, string sensorValue)
        {
            Timestamp = DateTime.UtcNow;
            SensorType = sensorType;
            SensorValue = sensorValue;
        }
    }
}