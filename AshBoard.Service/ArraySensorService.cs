using AshBoard.Application.DTOs.ArraySensor;
using AshBoard.Application.Interfaces;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshBoard.Infrastructure.Services
{
    public class ArraySensorService : IArraySensorService
    {
        private readonly AshBoardDbContext _context;

        public ArraySensorService(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<ArraySensorDto> CreateAsync(CreateArraySensorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NomeLocal) || dto.NomeLocal.Length > 50)
                throw new ArgumentException("NomeLocal é obrigatório e deve ter até 50 caracteres.");

            var array = new ArraySensor
            {
                NomeLocal = dto.NomeLocal
            };

            _context.ArraysSensores.Add(array);
            await _context.SaveChangesAsync();

            return new ArraySensorDto
            {
                Id = array.Id,
                NomeLocal = array.NomeLocal,
                SensoresIds = new()
            };
        }

        public async Task<List<ArraySensorDto>> GetAllAsync()
        {
            return await _context.ArraysSensores
                .Include(a => a.Sensores)
                .Select(a => new ArraySensorDto
                {
                    Id = a.Id,
                    NomeLocal = a.NomeLocal,
                    SensoresIds = a.Sensores.Select(s => s.Id).ToList()
                })
                .ToListAsync();
        }

        public async Task<ArraySensorDto?> GetByIdAsync(int id)
        {
            var array = await _context.ArraysSensores
                .Include(a => a.Sensores)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (array == null) return null;

            return new ArraySensorDto
            {
                Id = array.Id,
                NomeLocal = array.NomeLocal,
                SensoresIds = array.Sensores.Select(s => s.Id).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateArraySensorDto dto)
        {
            var array = await _context.ArraysSensores.FindAsync(id);
            if (array == null) return false;

            if (string.IsNullOrWhiteSpace(dto.NomeLocal) || dto.NomeLocal.Length > 50)
                throw new ArgumentException("NomeLocal é obrigatório e deve ter até 50 caracteres.");

            array.NomeLocal = dto.NomeLocal;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var array = await _context.ArraysSensores
                .Include(a => a.Sensores)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (array == null) return false;

            // Desassociar sensores (opcional, mas previne erro se houver FK)
            foreach (var sensor in array.Sensores)
            {
                sensor.ArraySensorId = null;
            }

            _context.ArraysSensores.Remove(array);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
