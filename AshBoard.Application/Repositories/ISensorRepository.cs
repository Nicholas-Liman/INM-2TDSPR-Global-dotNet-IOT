using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface ISensorRepository
    {
        Task<List<Sensor>> GetAllAsync();
        Task<Sensor?> GetByIdAsync(string id);
        Task CreateAsync(Sensor sensor);
        Task UpdateAsync(Sensor sensor);
        Task DeleteAsync(string id);

        Task<List<Sensor>> GetByIdsAsync(IEnumerable<string> ids);
    }
}
