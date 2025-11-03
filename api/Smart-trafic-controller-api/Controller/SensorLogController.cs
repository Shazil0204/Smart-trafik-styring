using Microsoft.AspNetCore.Mvc;
using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Services;

namespace Smart_trafic_controller_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorLogController(ISensorLogService sensorLogService) : ControllerBase
    {
        private readonly ISensorLogService _sensorLogService = sensorLogService;

        [HttpGet]
        public async Task<IActionResult> GetAllSensorLogs()
        {
            try
            {
                List<SensorLogResponseDTO> sensorLogs =
                    await _sensorLogService.GetAllSensorLogsAsync();
                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return NotFound("No sensor logs found.");
                }
                return Ok(sensorLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("timerange")]
        public async Task<IActionResult> GetSensorLogsByTimeRange(
            DateTime startTime,
            DateTime endTime
        )
        {
            try
            {
                List<SensorLogResponseDTO> sensorLogs =
                    await _sensorLogService.GetSensorLogsByTimeRangeAsync(startTime, endTime);
                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return NotFound("No sensor logs found in the specified time range.");
                }
                return Ok(sensorLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSensorLog([FromBody] CreateSensorLogRequestDTO createSensorLogDTO)
        {
            try
            {
                if (createSensorLogDTO == null)
                {
                    return BadRequest("Sensor log data is null.");
                }

                SensorLogResponseDTO createdSensorLog = await _sensorLogService.CreateSensorLogAsync(createSensorLogDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
