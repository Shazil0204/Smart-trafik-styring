using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smart_trafic_controller_api.Data;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Repositories
{
    public class SensorLogRepository(AppDbContext context) : ISensorLogRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<SensorLog>> GetAllSensorLogsAsync()
        {
            try
            {
                List<SensorLog> sensorLogs = await _context.SensorLogs.ToListAsync();
                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return new List<SensorLog>();
                }
                return sensorLogs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving sensor logs.", ex);
            }
        }

        public async Task<List<SensorLog>> GetSensorLogsByTimeRangeAsync(
            DateTime startTime,
            DateTime endTime
        )
        {
            try
            {
                List<SensorLog> sensorLogs = await _context
                    .SensorLogs.Where(sl => sl.Timestamp >= startTime && sl.Timestamp <= endTime)
                    .ToListAsync();
                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return new List<SensorLog>();
                }
                return sensorLogs;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while retrieving sensor logs by time range.",
                    ex
                );
            }
        }

        public async Task<SensorLog> CreateSensorLogAsync(SensorLog sensorLog)
        {
            try
            {
                _context.SensorLogs.Add(sensorLog);
                await _context.SaveChangesAsync();
                return sensorLog;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating a new sensor log.", ex);
            }
        }
    }
}
