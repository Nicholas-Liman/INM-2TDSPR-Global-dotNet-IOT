using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.DTOs.Leitura;
using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Domain.Entities;

namespace AshBoard.Service.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public async Task<List<SensorDto>> GetAllAsync()
        {
            var sensores = await _sensorRepository.GetAllAsync();

            return sensores.Select(s => new SensorDto
            {
                Id = s.Id,
                NomeLocal = s.NomeLocal,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Temperatura = s.Temperatura,
                NivelCO2 = s.NivelCO2,
                DirecaoVento = s.DirecaoVento,
                DataUltimaLeitura = s.DataUltimaLeitura,
                ArraySensorId = s.ArraySensorId
            }).ToList();
        }

        public async Task<SensorDto?> GetByIdAsync(string id)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            if (sensor == null) return null;

            return new SensorDto
            {
                Id = sensor.Id,
                NomeLocal = sensor.NomeLocal,
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude,
                Temperatura = sensor.Temperatura,
                NivelCO2 = sensor.NivelCO2,
                DirecaoVento = sensor.DirecaoVento,
                DataUltimaLeitura = sensor.DataUltimaLeitura,
                ArraySensorId = sensor.ArraySensorId
            };
        }

        public async Task<SensorDto> CreateAsync(CreateSensorDto dto)
        {
            var sensor = new Sensor
            {
                Id = dto.Id,
                NomeLocal = dto.NomeLocal,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            await _sensorRepository.CreateAsync(sensor);

            return new SensorDto
            {
                Id = sensor.Id,
                NomeLocal = sensor.NomeLocal,
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude
            };
        }

        public async Task<bool> UpdateAsync(string id, UpdateSensorDto dto)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            if (sensor == null) return false;

            sensor.NomeLocal = dto.NomeLocal;
            sensor.Latitude = dto.Latitude;
            sensor.Longitude = dto.Longitude;
            sensor.Temperatura = dto.Temperatura;
            sensor.NivelCO2 = dto.NivelCO2;
            sensor.DirecaoVento = dto.DirecaoVento;
            sensor.DataUltimaLeitura = dto.DataUltimaLeitura;
            sensor.ArraySensorId = dto.ArraySensorId;

            await _sensorRepository.UpdateAsync(sensor);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            if (sensor == null) return false;

            await _sensorRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AtualizarLeituraAsync(string id, LeituraSensorDto dto)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            if (sensor == null) return false;

            sensor.Temperatura = dto.Temperatura;
            sensor.NivelCO2 = dto.NivelCO2;
            sensor.DirecaoVento = dto.DirecaoVento;
            sensor.DataUltimaLeitura = dto.DataHora;

            await _sensorRepository.UpdateAsync(sensor);
            return true;
        }
    }
}
