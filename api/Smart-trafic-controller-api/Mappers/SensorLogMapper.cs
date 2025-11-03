using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Mappers
{
    public static class SensorLogMapper
    {
        public static SensorLog ToEntity(this CreateSensorLogRequestDTO dto)
        {
            return new SensorLog(dto.SensorType, dto.SensorValue);
        }

        public static SensorLogResponseDTO ToResponseDTO(this SensorLog sensorLog)
        {
            return new SensorLogResponseDTO(
                sensorLog.Timestamp,
                sensorLog.SensorType.ToString(),
                sensorLog.SensorValue
            );
        }

        public static List<SensorLogResponseDTO> ToResponseListDTO(this IEnumerable<SensorLog> sensorLogs)
        {
            return sensorLogs.Select(sl => sl.ToResponseDTO()).ToList();
        }
    }
}
