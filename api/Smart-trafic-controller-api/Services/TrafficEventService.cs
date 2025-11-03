using Smart_trafic_controller_api.DTOs.TrafficEvent;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Mappers;

namespace Smart_trafic_controller_api.Services
{
    public class TrafficEventService(ITrafficEventRepository trafficEventRepository): ITrafficEventService
    {
        private readonly ITrafficEventRepository _trafficEventRepository = trafficEventRepository;
        public async Task<bool> CreateTrafficEvent(TrafficEventRequestDTO trafficEventRequestDTO)
        {
            try
            {
                TrafficEvent trafficEvent = TrafficEventMapper.ToEntity(trafficEventRequestDTO);
                await _trafficEventRepository.CreateTrafficEventAsync(trafficEvent);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the traffic event.", ex);
            }
        }

        public async Task<List<TrafficEventResponseDTO>> GetAllTrafficEvents()
        {
            try
            {
                List<TrafficEvent> trafficEvents = await  _trafficEventRepository.GetAllTrafficEventsAsync();
                if (trafficEvents == null || trafficEvents.Count == 0)
                {
                    return new List<TrafficEventResponseDTO>();
                }
                return TrafficEventMapper.ToResponseListDTO(trafficEvents);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? "An error occured while retrieving trafficevents.", ex);
            }
        }

        public async Task<List<TrafficEventResponseDTO>> GetTrafficEventsByTimeRange(DateTime startTime, DateTime endTime)
        {
            try
            {
                if (startTime >= endTime)
                {
                    throw new ArgumentException("Start time must be earlier than end time.");
                }
                List<TrafficEvent> trafficEvents = await _trafficEventRepository.GetTrafficEventsByTimeRangeAsync(startTime, endTime);
                if (trafficEvents == null || trafficEvents.Count == 0)
                {
                    return new List<TrafficEventResponseDTO>();
                }
                return TrafficEventMapper.ToResponseListDTO(trafficEvents);
            }
            catch (ArgumentException argEx)
            {
                throw new Exception(argEx.Message, argEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ?? "An error occurred while retrieving traffic events by time range.", ex);
            }
        }
    }
}