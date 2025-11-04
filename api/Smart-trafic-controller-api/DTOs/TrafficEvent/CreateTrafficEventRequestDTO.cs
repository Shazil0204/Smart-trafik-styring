namespace Smart_trafic_controller_api.DTOs.TrafficEvent
{
    public class CreateTrafficEventRequestDTO(bool vehicleDetected)
    {
        public bool VehicleDetected { get; set; } = vehicleDetected;
    }
}