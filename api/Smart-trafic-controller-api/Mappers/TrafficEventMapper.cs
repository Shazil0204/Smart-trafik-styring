using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.DTOs.TrafficEvent;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Enums;
using Smart_trafic_controller_api.Services;

namespace Smart_trafic_controller_api.Mappers
{
    public static class TrafficEventMapper
    {
        public static TrafficEvent ToEntity(this CreateTrafficEventRequestDTO dto)
        {
            return new TrafficEvent(
                dto.VehicleDetected
            );
        }

        public static TrafficEventResponseDTO ToResponseDTO(this TrafficEvent trafficEvent)
        {
            return new TrafficEventResponseDTO
            {
                TimeStamp = trafficEvent.TimeStamp,
                VehicleDetected = trafficEvent.VehicleDetected,
                TrafficLight = trafficEvent.TrafficLight
                // You could include more fields later if needed
            };
        }

        public static List<TrafficEventResponseDTO> ToResponseListDTO(this List<TrafficEvent> trafficEvents)
        {
            return trafficEvents.Select(te => te.ToResponseDTO()).ToList();
        }

        public static CreateTrafficEventRequestDTO ToCreateTrafficEventRequestDTO(this CreateSensorLogRequestDTO sensorLogRequestDTO)
        {
            if (sensorLogRequestDTO.SensorValue == SensorValue.VEHICLE_RED)
            {
                return new CreateTrafficEventRequestDTO(false);
            }
            else
            {
                return new CreateTrafficEventRequestDTO(true);
            }
        }
    }
}
