using Smart_trafic_controller_api.Entities;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ITrafficEventRepository
    {
        Task<List<TrafficEvent>> GetAllTrafficEventsAsync();
        Task<List<TrafficEvent>> GetTrafficEventsByTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<(TrafficEvent? traffciEvent, int count)> GetLatestTrafficEventAsync();
        Task<TrafficEvent> CreateTrafficEventAsync(TrafficEvent trafficEvent);
    }
}