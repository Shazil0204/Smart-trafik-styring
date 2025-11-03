using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.TrafficEvent;
using Smart_trafic_controller_api.Interfaces;

namespace Smart_trafic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrafficEventController(ITrafficEventService trafficEventService) : ControllerBase
    {
        private readonly ITrafficEventService _trafficEventService = trafficEventService;

        [HttpGet]
        public async Task<IActionResult> GetAllTrafficEvents()
        {
            try
            {
                List<TrafficEventResponseDTO> trafficEvents = await _trafficEventService.GetAllTrafficEvents();
                if (trafficEvents == null)
                {
                    return NotFound("No traffic events found.");
                }
                return Ok(trafficEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("timerange")]
        public async Task<IActionResult> GetTrafficEventsByTimeRange(DateTime startTime, DateTime endTime)
        {
            try
            {
                List<TrafficEventResponseDTO> trafficEvents = await _trafficEventService.GetTrafficEventsByTimeRange(startTime, endTime);
                if (trafficEvents == null)
                {
                    return NotFound("No traffic events found in the specified time range.");
                }
                return Ok(trafficEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}