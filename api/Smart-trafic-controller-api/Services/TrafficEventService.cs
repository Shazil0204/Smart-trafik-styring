using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.DTOs;

namespace Smart_trafic_controller_api.Services
{
    public class TrafficEventService
    {
        public bool CreateTrafficEvent(TrafficEventRequestDTO trafficEventRequestDTO)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the traffic event.", ex);
            }
        }
    }
}