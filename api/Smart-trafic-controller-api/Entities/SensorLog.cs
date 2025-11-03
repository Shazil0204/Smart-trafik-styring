using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Entities
{
    public class SensorLog
    {
        public int Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public SensorType SensorType { get; private set; }
        public string SensorValue { get; private set; } = null!;

        private SensorLog() { } // For EF Core

        public SensorLog(SensorType sensorType, string sensorValue)
        {
            Timestamp = DateTime.UtcNow;
            SensorType = sensorType;
            SensorValue = sensorValue;
        }
    }
}