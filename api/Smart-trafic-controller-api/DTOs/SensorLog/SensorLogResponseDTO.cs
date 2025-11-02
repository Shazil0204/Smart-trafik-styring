using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.DTOs.SensorLog
{
    public class SensorLogResponseDTO(DateTime timeStamp, string sensorType, string sensorValue)
    {
        public DateTime TimeStamp { get; set; } = timeStamp;
        public string SensorType { get; set; } = sensorType;
        public string SensorValue { get; set; } = sensorValue;
    }
}
