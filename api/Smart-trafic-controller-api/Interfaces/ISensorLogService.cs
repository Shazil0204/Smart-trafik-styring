using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ISensorLogService
    {
        Task<List<SensorLogResponseDTO>> GetAllSensorLogsAsync();
        Task<List<SensorLogResponseDTO>> GetSensorLogsByTimeRangeAsync(
            DateTime startTime,
            DateTime endTime
        );
        Task CreateSensorLogAsync(SensorValue sensorValue);
    }
}
