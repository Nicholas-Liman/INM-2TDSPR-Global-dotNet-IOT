using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.Repositories;
using AshBoard.Application.Interfaces;
using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AshBoard.Infrastructure.Services
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
            var entities = await _sensorRepository.GetAllAsync();
            return entities.Select(s => new SensorDto
            {
                Id = s.Id,
                Nome = s.Nome,
                Latitude = s.Latitude,
                Longitude = s.Longitude
            }).ToList();
        }

        public async Task<SensorDto> GetByIdAsync(int id)
        {
            var s = await _sensorRepository.GetByIdAsync(id);
            if (s == null) return null;

            return new SensorDto
            {
                Id = s.Id,
                Nome = s.Nome,
                Latitude = s.Latitude,
                Longitude = s.Longitude
            };
        }

        public async Task CreateAsync(CreateSensorDto dto)
        {
            var entity = new SensorEntity
            {
                Nome = dto.Nome,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };
            await _sensorRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, UpdateSensorDto dto)
        {
            var existing = await _sensorRepository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Nome = dto.Nome;
            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;

            await _sensorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _sensorRepository.DeleteAsync(id);
        }
    }
}
