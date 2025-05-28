using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Infrastructure.Repositories
{
    public class ArraySensorRepository : IArraySensorRepository
    {
        private readonly AshBoardDbContext _context;

        public ArraySensorRepository(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArraySensorEntity>> GetAllAsync()
        {
            return await _context.ArraySensores
                .Include(a => a.Sensores) // Assume que existe navegação para sensores
                .ToListAsync();
        }

        public async Task<ArraySensorEntity> GetByIdAsync(int id)
        {
            return await _context.ArraySensores
                .Include(a => a.Sensores)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAsync(ArraySensorEntity arraySensor)
        {
            _context.ArraySensores.Add(arraySensor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ArraySensorEntity arraySensor)
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
