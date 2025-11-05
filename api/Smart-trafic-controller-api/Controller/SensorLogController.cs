using Microsoft.AspNetCore.Mvc;
using Smart_traffic_controller_api.DTOs.SensorLog;
using Smart_traffic_controller_api.Interfaces;

namespace Smart_traffic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorLogController(ISensorLogService sensorLogService) : ControllerBase
    {
        private readonly ISensorLogService _sensorLogService = sensorLogService;

        [HttpGet]
        public async Task<IActionResult> GetAllSensorLogs()
        {
            List<SensorLogResponseDTO> sensorLogs = await _sensorLogService.GetAllSensorLogsAsync();

            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return NotFound("No sensor logs found.");
            }

            return Ok(sensorLogs);
        }

        [HttpGet("timerange")]
        public async Task<IActionResult> GetSensorLogsByTimeRange(
            DateTime startTime,
            DateTime endTime
        )
        {
            List<SensorLogResponseDTO> sensorLogs =
                await _sensorLogService.GetSensorLogsByTimeRangeAsync(startTime, endTime);

            if (sensorLogs == null || sensorLogs.Count == 0)
            {
                return NotFound("No sensor logs found in the specified time range.");
            }

            return Ok(sensorLogs);
        }

        [HttpGet("average-traffic-light-duration")]
        public async Task<IActionResult> GetAverageTrafficLightDuration()
        {
            float averageDuration =
                await _sensorLogService.GetAverageTrafficLightDurationAsync();

            if (averageDuration < 0)
            {
                return NotFound("No traffic light duration data available.");
            }

            return Ok(averageDuration);
        }
    }
}
