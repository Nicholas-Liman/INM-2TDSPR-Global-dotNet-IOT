using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using AshBoard.Application.Repositories;
using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AshBoard.Service.Services
{
    public class ArraySensorService : IArraySensorService
    {
        private readonly IArraySensorRepository _arraySensorRepository;
        private readonly ISensorRepository _sensorRepository;

        public ArraySensorService(IArraySensorRepository arraySensorRepository, ISensorRepository sensorRepository)
        {
            _arraySensorRepository = arraySensorRepository;
            _sensorRepository = sensorRepository;
        }

        public async Task<List<ArraySensorDto>> GetAllAsync()
        {
            var arrays = await _arraySensorRepository.GetAllAsync();

            return arrays.Select(array => new ArraySensorDto
            {
                Id = array.Id,
                NomeLocal = array.NomeLocal,
                SensoresIds = array.Sensores?.Select(s => s.Id).ToList() ?? new List<string>()
            }).ToList();
        }

        public async Task<ArraySensorDto?> GetByIdAsync(int id)
        {
            var array = await _arraySensorRepository.GetByIdAsync(id);
            if (array == null) return null;

            return new ArraySensorDto
            {
                Id = array.Id,
                NomeLocal = array.NomeLocal,
                SensoresIds = array.Sensores?.Select(s => s.Id).ToList() ?? new List<string>()
            };
        }

        public async Task<ArraySensorDto> CreateAsync(CreateArraySensorDto dto)
        {
            if (dto.SensorIds == null || !dto.SensorIds.Any())
                throw new ArgumentException("Nenhum SensorId foi informado.");

            var sensores = await _sensorRepository.GetAllAsync();
            var sensoresSelecionados = sensores.Where(s => dto.SensorIds.Contains(s.Id)).ToList();

            var encontrados = sensoresSelecionados.Select(s => s.Id).ToHashSet();
            var naoEncontrados = dto.SensorIds.Where(id => !encontrados.Contains(id)).ToList();

            if (naoEncontrados.Any())
                throw new ArgumentException($"Os seguintes SensorIds não existem: {string.Join(", ", naoEncontrados)}");

            var arraySensor = new ArraySensor
            {
                NomeLocal = dto.NomeLocal,
                Sensores = sensoresSelecionados
            };

            await _arraySensorRepository.CreateAsync(arraySensor);

            return new ArraySensorDto
            {
                Id = arraySensor.Id,
                NomeLocal = arraySensor.NomeLocal,
                SensoresIds = sensoresSelecionados.Select(s => s.Id).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateArraySensorDto dto)
        {
            var existing = await _arraySensorRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.NomeLocal = dto.NomeLocal;
            await _arraySensorRepository.UpdateAsync(existing);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _arraySensorRepository.DeleteAsync(id);
            return true;
        }
    }
}
