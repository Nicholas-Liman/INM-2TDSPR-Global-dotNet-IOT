using AshBoard.Application.DTOs.Sensor;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Infrastructure.Services
{
    public class SensorService : ISensorService
    {
        private readonly AshBoardDbContext _context;

        public SensorService(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<SensorDto> CreateAsync(CreateSensorDto dto)
        {
            var idExistente = await _context.Sensores.AnyAsync(s => s.Id == dto.Id);
            if (idExistente)
                throw new ArgumentException("Já existe um sensor com este ID.");

            if (dto.Latitude < -90 || dto.Latitude > 90)
                throw new ArgumentException("Latitude deve estar entre -90 e 90.");

            if (dto.Longitude < -180 || dto.Longitude > 180)
                throw new ArgumentException("Longitude deve estar entre -180 e 180.");

            if (string.IsNullOrWhiteSpace(dto.NomeLocal) || dto.NomeLocal.Length > 50)
                throw new ArgumentException("NomeLocal é obrigatório e deve ter até 50 caracteres.");

            var sensor = new Sensor
            {
                Id = dto.Id,
                NomeLocal = dto.NomeLocal,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            _context.Sensores.Add(sensor);
            await _context.SaveChangesAsync();

            return new SensorDto
            {
                Id = sensor.Id,
                NomeLocal = sensor.NomeLocal,
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude
            };
        }

        public async Task<List<SensorDto>> GetAllAsync()
        {
            return await _context.Sensores
                .Select(s => new SensorDto
                {
                    Id = s.Id,
                    NomeLocal = s.NomeLocal,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Temperatura = s.Temperatura,
                    NivelCO2 = s.NivelCO2,
                    DirecaoVento = s.DirecaoVento,
                    ArraySensorId = s.ArraySensorId
                })
                .ToListAsync();
        }

        public async Task<SensorDto?> GetByIdAsync(string id)
        {
            var sensor = await _context.Sensores.FindAsync(id);
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
                ArraySensorId = sensor.ArraySensorId
            };
        }

        public async Task<bool> UpdateAsync(string id, UpdateSensorDto dto)
        {
            var sensor = await _context.Sensores.FindAsync(id);
            if (sensor == null) return false;

            if (dto.Latitude < -90 || dto.Latitude > 90)
                throw new ArgumentException("Latitude deve estar entre -90 e 90.");

            if (dto.Longitude < -180 || dto.Longitude > 180)
                throw new ArgumentException("Longitude deve estar entre -180 e 180.");

            if (string.IsNullOrWhiteSpace(dto.NomeLocal) || dto.NomeLocal.Length > 50)
                throw new ArgumentException("NomeLocal é obrigatório e deve ter até 50 caracteres.");

            sensor.NomeLocal = dto.NomeLocal;
            sensor.Latitude = dto.Latitude;
            sensor.Longitude = dto.Longitude;
            sensor.Temperatura = dto.Temperatura;
            sensor.NivelCO2 = dto.NivelCO2;
            sensor.DirecaoVento = dto.DirecaoVento;
            sensor.ArraySensorId = dto.ArraySensorId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var sensor = await _context.Sensores.FindAsync(id);
            if (sensor == null) return false;

            _context.Sensores.Remove(sensor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
