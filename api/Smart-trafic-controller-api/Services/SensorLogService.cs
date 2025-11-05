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

        public async Task<float> GetAverageTrafficLightDurationAsync()
        {
            List<SensorLog> sensorLogs = await _sensorLogRepository.GetAllSensorLogsAsync();

            List<float> eachTrafficLightDurations = [];

            DateTime? greenStartTime = null;

            foreach (var log in sensorLogs)
            {
                if (log.SensorValue == SensorValue.VEHICLE_GREEN && greenStartTime == null)
                {
                    greenStartTime = log.Timestamp;
                }
                else if (log.SensorValue == SensorValue.VEHICLE_RED && greenStartTime != null)
                {
                    TimeSpan duration = log.Timestamp - greenStartTime.Value;
                    eachTrafficLightDurations.Add((float)duration.TotalSeconds);
                    greenStartTime = null;
                }
            }

            if (eachTrafficLightDurations.Count == 0)
            {
                return 0;
            }

            return eachTrafficLightDurations.Average();
        }
    }
}
