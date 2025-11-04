using Smart_trafic_controller_api.DTOs.TrafficEvent;

namespace Smart_trafic_controller_api.Interfaces
{
    public interface ITrafficEventService
    {
        Task<bool> CreateTrafficEvent(CreateTrafficEventRequestDTO trafficEventRequestDTO);
        Task<List<TrafficEventResponseDTO>> GetAllTrafficEvents();
        Task<List<TrafficEventResponseDTO>> GetTrafficEventsByTimeRange(DateTime startTime, DateTime endTime);
        
    }
}