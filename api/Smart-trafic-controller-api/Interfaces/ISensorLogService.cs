using Smart_trafic_controller_api.DTOs.SensorLog;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ISensorLogService
    {
        Task<List<SensorLogResponseDTO>> GetAllSensorLogsAsync();
        Task<List<SensorLogResponseDTO>> GetSensorLogsByTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<SensorLogResponseDTO> CreateSensorLogAsync(CreateSensorLogRequestDTO createSensorLogDTO);
    }
}
