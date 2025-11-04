using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.DTOs.SensorLog
{
    public class SensorLogResponseDTO(DateTime timeStamp, SensorValue sensorValue)
    {
        public DateTime TimeStamp { get; set; } = timeStamp;
        public SensorValue SensorValue { get; set; } = sensorValue;
    }
}
