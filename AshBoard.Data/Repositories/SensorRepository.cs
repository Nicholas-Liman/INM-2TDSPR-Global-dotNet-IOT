using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AshBoard.Data.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly AshBoardDbContext _context;

        public SensorRepository(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sensor>> GetAllAsync()
        {
            return await _context.Sensores.ToListAsync();
        }

        public async Task<Sensor?> GetByIdAsync(string id)
        {
            return await _context.Sensores.FindAsync(id);
        }

        public async Task CreateAsync(Sensor sensor)
        {
            _context.Sensores.Add(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sensor sensor)
        {
            _context.Sensores.Update(sensor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var sensor = await _context.Sensores.FindAsync(id);
            if (sensor != null)
            {
                _context.Sensores.Remove(sensor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Sensor>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _context.Sensores
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
