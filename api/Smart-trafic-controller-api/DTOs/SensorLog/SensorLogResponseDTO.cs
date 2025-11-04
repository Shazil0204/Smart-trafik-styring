using Smart_traffic_controller_api.Enums;

namespace Smart_traffic_controller_api.DTOs.SensorLog
{
    public class SensorLogResponseDTO(DateTime timeStamp, SensorValue sensorValue)
    {
        public DateTime TimeStamp { get; set; } = timeStamp;
        public string SensorValue { get; set; } = sensorValue.ToString();
    }
}
