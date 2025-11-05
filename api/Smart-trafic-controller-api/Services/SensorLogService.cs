using Smart_traffic_controller_api.DTOs.SensorLog;
using Smart_traffic_controller_api.Entities;
using Smart_traffic_controller_api.Enums;
using Smart_traffic_controller_api.Interfaces;
using Smart_traffic_controller_api.Mappers;

namespace Smart_traffic_controller_api.Services
{
    public class SensorLogService(ISensorLogRepository sensorLogRepository) : ISensorLogService
    {
        private readonly ISensorLogRepository _sensorLogRepository = sensorLogRepository;

        public async Task<List<SensorLogResponseDTO>> GetAllSensorLogsAsync()
        {
            List<SensorLog> sensorLogs = await _sensorLogRepository.GetAllSensorLogsAsync();

            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return new List<SensorLogResponseDTO>();
            }

            return SensorLogMapper.ToResponseListDTO(sensorLogs);
        }

        public async Task<List<SensorLogResponseDTO>> GetSensorLogsByTimeRangeAsync(
            DateTime startTime,
            DateTime endTime
        )
        {
            List<SensorLog> sensorLogs = await _sensorLogRepository.GetSensorLogsByTimeRangeAsync(
                startTime,
                endTime
            );

            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return new List<SensorLogResponseDTO>();
            }

            return SensorLogMapper.ToResponseListDTO(sensorLogs);
        }

        public async Task CreateSensorLogAsync(SensorValue sensorValue)
        {
            if (sensorValue.GetType() != typeof(SensorValue))
            {
                throw new ArgumentException(
                    "Sensor type and sensor value cannot be null or empty."
                );
            }

            await _sensorLogRepository.CreateSensorLogAsync(new SensorLog(sensorValue));
        }
    }
}
