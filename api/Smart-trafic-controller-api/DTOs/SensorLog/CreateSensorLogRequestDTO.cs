using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.DTOs.SensorLog
{
    public class CreateSensorLogRequestDTO(SensorValue sensorValue)
    {
        public SensorValue SensorValue { get; set; } = sensorValue;
    }
}