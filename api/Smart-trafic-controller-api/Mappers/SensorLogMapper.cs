using System;
using System.Collections.Generic;
using System.Linq;
using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Mappers
{
    public static class SensorLogMapper
    {
        public static SensorLog ToEntity(this CreateSensorLogRequestDTO dto)
        {
            // Convert the string to enum (case-insensitive)
            if (!Enum.TryParse(dto.SensorType, true, out SensorType sensorType))
            {
                throw new ArgumentException($"Invalid sensor type: {dto.SensorType}");
            }

            return new SensorLog(sensorType, dto.SensorValue);
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
