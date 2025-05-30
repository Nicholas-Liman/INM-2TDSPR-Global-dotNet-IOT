using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface IAlertaRepository
    {
        Task<List<Alerta>> GetAllAsync();
        Task<Alerta> GetByIdAsync(int id);
        Task CreateAsync(Alerta alerta);
        Task UpdateAsync(Alerta alerta);
        Task DeleteAsync(int id);
    }
}
