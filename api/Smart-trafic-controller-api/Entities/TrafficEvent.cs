
namespace Smart_trafic_controller_api.Entities
{
    public class TrafficEvent
    {
        public Guid Id { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public bool VehicleDetected { get; private set; }
        public bool TrafficLight { get; private set; }
        public DateTime Duration { get; private set; }
        private TrafficEvent() { } // For EF Core

        // TODO:
        // In service, check the latest created trafficevent, and see if it is more than
        // seconds ago.
        public TrafficEvent(bool vehicleDetected)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            VehicleDetected = vehicleDetected;
            TrafficLight = vehicleDetected;
            Duration = DateTime.UtcNow.AddSeconds(5);
        }
    }
}