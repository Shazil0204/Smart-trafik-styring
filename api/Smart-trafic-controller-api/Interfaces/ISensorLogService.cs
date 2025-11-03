using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ISensorLogService
    {
        Task<List<SensorLogResponseDTO>> GetAllSensorLogsAsync();
        Task<List<SensorLogResponseDTO>> GetSensorLogsByTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<SensorLogResponseDTO> CreateSensorLogAsync(CreateSensorLogRequestDTO createSensorLogDTO);
    }
}
