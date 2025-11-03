using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.DTOs.SensorLog
{
    public class CreateSensorLogRequestDTO(SensorType sensorType, string sensorValue)
    {
        public SensorType SensorType { get; set; } = sensorType;
        public string SensorValue { get; set; } = sensorValue;
    }
}