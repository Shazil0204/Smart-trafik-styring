namespace Smart_trafic_controller_api.DTOs.TrafficEvent
{
    public class TrafficEventRequestDTO
    {
        public string EventType { get; set; } = null!;
        public string EventValue { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
    }
}