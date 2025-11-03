using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smart_trafic_controller_api.DTOs.SensorLog;
using Smart_trafic_controller_api.Entities;
using Smart_trafic_controller_api.Enums;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Mappers;
using Smart_trafic_controller_api.Repositories;

namespace Smart_trafic_controller_api.Services
{
    public class SensorlogService(ISensorLogRepository sensorLogRepository, ITrafficEventService trafficEventService) : ISensorLogService
    {
        private readonly ISensorLogRepository _sensorLogRepository = sensorLogRepository;
        private readonly ITrafficEventService _trafficEventService = trafficEventService;

        public async Task<List<SensorLogResponseDTO>> GetAllSensorLogsAsync()
        {
            try
            {
                List<SensorLog> sensorLogs = await _sensorLogRepository.GetAllSensorLogsAsync();

                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return new List<SensorLogResponseDTO>();
                }
                return SensorLogMapper.ToResponseListDTO(sensorLogs);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all sensor logs.", ex);
            }
        }

        public async Task<List<SensorLogResponseDTO>> GetSensorLogsByTimeRangeAsync(
            DateTime startTime,
            DateTime endTime
        )
        {
            try
            {
                List<SensorLog> sensorLogs =
                    await _sensorLogRepository.GetSensorLogsByTimeRangeAsync(startTime, endTime);

                if (sensorLogs == null || sensorLogs.Count == 0)
                {
                    return new List<SensorLogResponseDTO>();
                }

                return SensorLogMapper.ToResponseListDTO(sensorLogs);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while retrieving sensor logs by time range.",
                    ex
                );
            }
        }

        public async Task<SensorLogResponseDTO> CreateSensorLogAsync(CreateSensorLogRequestDTO createSensorLogDTO)
        {
            try
            {
                if (createSensorLogDTO == null)
                {
                    throw new ArgumentNullException(
                        nameof(createSensorLogDTO),
                        "Sensor log cannot be null."
                    );
                }
                if (string.IsNullOrEmpty(createSensorLogDTO.SensorValue))
                {
                    throw new ArgumentException(
                        "Sensor type and sensor value cannot be null or empty."
                    );
                }

                // TODO:
                // SensorLog analyze to create trafficevent 
                // if (createSensorLogDTO.SensorType == SensorType.Pedestrian)
                // {
                    
                // }

                SensorLog createdSensorLog = await _sensorLogRepository.CreateSensorLogAsync(
                    SensorLogMapper.ToEntity(createSensorLogDTO)
                );
                return SensorLogMapper.ToResponseDTO(createdSensorLog);
            }
            catch (ArgumentNullException argEx)
            {
                throw new ArgumentNullException(argEx.Message, argEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException(argEx.Message, argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the sensor log.", ex);
            }
        }
    }
}
