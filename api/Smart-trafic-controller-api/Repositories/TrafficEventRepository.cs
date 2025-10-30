using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Data;
using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Repositories
{
    public class TrafficEventRepository(AppDbContext context)
    {
        public readonly AppDbContext _context = context;

        public async Task<List<TrafficEvent>> GetAllTrafficEventsAsync()
        {
            try
            {
                return _context.TrafficEvents.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving traffic events.", ex);
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