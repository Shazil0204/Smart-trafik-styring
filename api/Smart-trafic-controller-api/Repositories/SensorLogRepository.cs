using Microsoft.EntityFrameworkCore;
using Smart_traffic_controller_api.Data;
using Smart_traffic_controller_api.Entities;
using Smart_traffic_controller_api.Interfaces;

namespace Smart_traffic_controller_api.Repositories
{
    public class SensorLogRepository(AppDbContext context) : ISensorLogRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<SensorLog>> GetAllSensorLogsAsync()
        {
            List<SensorLog> sensorLogs = await _context.SensorLogs.ToListAsync();
            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return new List<SensorLog>();
            }

            return sensorLogs;
        }

        public async Task<List<SensorLog>> GetSensorLogsByTimeRangeAsync(
            DateTime startTime,
            DateTime endTime
        )
        {
            // Finds all the sensorlogs between the two timestamps
            List<SensorLog> sensorLogs = await _context
                .SensorLogs.Where(sl => sl.Timestamp >= startTime && sl.Timestamp <= endTime)
                .ToListAsync();
            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return new List<SensorLog>();
            }

            return sensorLogs;
        }

        public async Task<SensorLog> CreateSensorLogAsync(SensorLog sensorLog)
        {
            _context.SensorLogs.Add(sensorLog);
            await _context.SaveChangesAsync();
            return sensorLog;
        }
    }
}
