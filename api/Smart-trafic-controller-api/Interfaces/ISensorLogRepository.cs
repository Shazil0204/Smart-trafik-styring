using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ISensorLogRepository
    {
        Task<List<SensorLog>> GetAllSensorLogsAsync();
        Task<List<SensorLog>> GetSensorLogsByTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<SensorLog> CreateSensorLogAsync(SensorLog sensorLog);
    }
}