using Smart_trafic_controller_api.DTOs.TrafficEvent;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Mappers
{
    public static class TrafficEventMapper
    {
        public static TrafficEvent ToEntity(this TrafficEventRequestDTO dto)
        {
            bool vehicleDetected = dto.EventType.Equals("VehicleDetected", StringComparison.OrdinalIgnoreCase);
            bool pedestrianDetected = dto.EventType.Equals("PedestrianDetected", StringComparison.OrdinalIgnoreCase);

            // Attempt to parse EventValue for light statuses or duration
            Enum.TryParse(dto.EventValue, true, out vehicleLightStatus vehicleLight);
            Enum.TryParse(dto.EventValue, true, out PedestrainLightStatus pedLight);

            int.TryParse(dto.EventValue, out int durationValue);

            return new TrafficEvent(
                dto.TimeStamp,
                vehicleDetected,
                pedestrianDetected,
                vehicleLight,
                pedLight,
                durationValue
            );
        }

        public static TrafficEventResponseDTO ToResponseDTO(this TrafficEvent trafficEvent)
        {
            return new TrafficEventResponseDTO
            {
                Id = trafficEvent.Id
                // You could include more fields later if needed
            };
        }

        public static List<TrafficEventResponseDTO> ToResponseListDTO(this List<TrafficEvent> trafficEvents)
        {
            return trafficEvents.Select(te => te.ToResponseDTO()).ToList();
        }
    }
}
