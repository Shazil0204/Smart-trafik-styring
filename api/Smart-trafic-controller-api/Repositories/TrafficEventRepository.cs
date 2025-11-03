using Microsoft.EntityFrameworkCore;
using Smart_trafic_controller_api.Data;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Repositories
{
    public class TrafficEventRepository(AppDbContext context): ITrafficEventRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<TrafficEvent>> GetAllTrafficEventsAsync()
        {
            try
            {
                List<TrafficEvent> trafficEvents = await _context.TrafficEvents.ToListAsync();
                if (trafficEvents == null || trafficEvents.Count == 0)
                {
                    return new List<TrafficEvent>();
                }
                return trafficEvents;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving traffic events.", ex);
            }
        }

        public async Task<List<TrafficEvent>> GetTrafficEventsByTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            try
            {
                List<TrafficEvent> trafficEvents = await _context.TrafficEvents
                    .Where(te => te.TimeStamp >= startTime && te.TimeStamp <= endTime)
                    .ToListAsync();
                if (trafficEvents == null || trafficEvents.Count == 0)
                {
                    return new List<TrafficEvent>();
                }
                return trafficEvents;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving traffic events by time range.", ex);
            }
        }

        public async Task<TrafficEvent> CreateTrafficEventAsync(TrafficEvent trafficEvent)
        {
            try
            {
                _context.TrafficEvents.Add(trafficEvent);
                await _context.SaveChangesAsync();
                return trafficEvent;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the traffic event.", ex);
            }
        }

    }
}