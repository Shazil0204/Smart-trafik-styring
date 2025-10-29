using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.Enums;

namespace Smart_trafic_controller_api.Entities
{
    public class TrafficEvent
    {
        public Guid Id { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public bool VehicleDetected { get; private set; }
        public bool PedestrianDetected { get; private set; }
        public vehicleLightStatus VehicleLightStatus { get; private set; }
        public PedestrainLightStatus PedLightStatus { get; private set; }
        public int Duration { get; private set; }
        private TrafficEvent() { } // For EF Core
        public TrafficEvent(Guid id, DateTime timeStamp, bool vehicleDetected, bool pedestrianDetected, vehicleLightStatus vehicleLightStatus, PedestrainLightStatus pedLightStatus, int duration)
        {
            Id = id;
            TimeStamp = timeStamp;
            VehicleDetected = vehicleDetected;
            PedestrianDetected = pedestrianDetected;
            VehicleLightStatus = vehicleLightStatus;
            PedLightStatus = pedLightStatus;
            Duration = duration;
        }
    }
}