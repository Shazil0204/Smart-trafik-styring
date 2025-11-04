namespace Smart_trafic_controller_api.DTOs.TrafficEvent
{
    public class TrafficEventResponseDTO
    {
        public DateTime TimeStamp { get; set; }
        public bool VehicleDetected { get; set; }
        public bool TrafficLight { get; set; }
    }
}