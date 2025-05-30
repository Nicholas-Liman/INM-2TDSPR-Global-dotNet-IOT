using AshBoard.Application.Repositories;
using AshBoard.Data.AppData;
using AshBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Data.Repositories
{
    public class AlertaRepository : IAlertaRepository
    {
        private readonly AshBoardDbContext _context;

        public AlertaRepository(AshBoardDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alerta>> GetAllAsync()
        {
            return await _context.Alertas.ToListAsync();
        }

        public async Task<Alerta?> GetByIdAsync(int id)
        {
            return await _context.Alertas.FindAsync(id);
        }

        public async Task CreateAsync(Alerta alerta)
        {
            _context.Alertas.Add(alerta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Alerta alerta)
        {
            _context.Alertas.Update(alerta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta != null)
            {
                _context.Alertas.Remove(alerta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
