using Smart_traffic_controller_api.Entities;

namespace Smart_traffic_controller_api.Interfaces
{
    public interface ISensorLogRepository
    {
        Task<List<SensorLog>> GetAllSensorLogsAsync();
        Task<List<SensorLog>> GetSensorLogsByTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<SensorLog> CreateSensorLogAsync(SensorLog sensorLog);
    }
}
