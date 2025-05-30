using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Data.Repositories
{
    public class ArraySensorRepository : IArraySensorRepository
    {
        private readonly AshBoardDbContext _context;

        public ArraySensorRepository(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArraySensor>> GetAllAsync()
        {
            return await _context.ArraySensores
                .Include(a => a.Sensores)
                .ToListAsync();
        }

        public async Task<ArraySensor> GetByIdAsync(int id)
        {
            var arraySensor = await _context.ArraySensores
                .Include(a => a.Sensores)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (arraySensor == null)
                throw new KeyNotFoundException($"ArraySensor with ID {id} not found.");

            return arraySensor;
        }

        public async Task CreateAsync(ArraySensor arraySensor)
        {
            _context.ArraySensores.Add(arraySensor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ArraySensor arraySensor)
        {
            _context.ArraySensores.Update(arraySensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var array = await _context.ArraySensores.FindAsync(id);
            if (array != null)
            {
                _context.ArraySensores.Remove(array);
                await _context.SaveChangesAsync();
            }
        }
    }
}
