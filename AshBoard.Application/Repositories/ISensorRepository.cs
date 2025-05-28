using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface ISensorRepository
    {
        Task<List<SensorEntity>> GetAllAsync();
        Task<SensorEntity> GetByIdAsync(int id);
        Task CreateAsync(SensorEntity sensor);
        Task UpdateAsync(SensorEntity sensor);
        Task DeleteAsync(int id);
    }
}
