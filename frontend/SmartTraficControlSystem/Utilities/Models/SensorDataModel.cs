using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTraficControlSystem.Utilities.Models
{
    public class SensorDataModel
    {
        public DateTime Timestamp { get; set; }
        public string SensorValue { get; set; }
    }
}