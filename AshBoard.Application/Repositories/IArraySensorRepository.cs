using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface IArraySensorRepository
    {
        Task<List<ArraySensorEntity>> GetAllAsync();
        Task<ArraySensorEntity> GetByIdAsync(int id);
        Task CreateAsync(ArraySensorEntity arraySensor);
        Task UpdateAsync(ArraySensorEntity arraySensor);
        Task DeleteAsync(int id);
    }
}
