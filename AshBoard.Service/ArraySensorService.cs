using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AshBoard.Infrastructure.Services
{
    public class ArraySensorService : IArraySensorService
    {
        private readonly IArraySensorRepository _arraySensorRepository;

        public ArraySensorService(IArraySensorRepository arraySensorRepository)
        {
            _arraySensorRepository = arraySensorRepository;
        }

        public async Task<List<ArraySensorDto>> GetAllAsync()
        {
            var arrays = await _arraySensorRepository.GetAllAsync();

            return arrays.Select(array => new ArraySensorDto
            {
                Id = array.Id,
                Nome = array.Nome,
                Sensores = array.Sensores?.Select(sensor => new SensorLeituraDto
                {
                    Id = sensor.Id,
                    Nome = sensor.Nome,
                    Temperatura = sensor.Temperatura,
                    Carbono = sensor.Carbono,
                    DirecaoVento = sensor.DirecaoVento
                }).ToList()
            }).ToList();
        }

        public async Task<ArraySensorDto> GetByIdAsync(int id)
        {
            var array = await _arraySensorRepository.GetByIdAsync(id);
            if (array == null) return null;

            return new ArraySensorDto
            {
                Id = array.Id,
                Nome = array.Nome,
                Sensores = array.Sensores?.Select(sensor => new SensorLeituraDto
                {
                    Id = sensor.Id,
                    Nome = sensor.Nome,
                    Temperatura = sensor.Temperatura,
                    Carbono = sensor.Carbono,
                    DirecaoVento = sensor.DirecaoVento
                }).ToList()
            };
        }

        public async Task CreateAsync(CreateArraySensorDto dto)
        {
            var array = new ArraySensorEntity
            {
                Nome = dto.Nome
                // Associe sensores aqui se necessário
            };

            await _arraySensorRepository.CreateAsync(array);
        }

        public async Task UpdateAsync(int id, UpdateArraySensorDto dto)
        {
            var existing = await _arraySensorRepository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Nome = dto.Nome;
            await _arraySensorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _arraySensorRepository.DeleteAsync(id);
        }
    }
}
