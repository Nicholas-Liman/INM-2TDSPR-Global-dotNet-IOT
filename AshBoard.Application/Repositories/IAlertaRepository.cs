using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface IAlertaRepository
    {
        Task<List<AlertaEntity>> GetAllAsync();
        Task<AlertaEntity> GetByIdAsync(int id);
        Task CreateAsync(AlertaEntity alerta);
        Task UpdateAsync(AlertaEntity alerta);
        Task DeleteAsync(int id);
    }
}
