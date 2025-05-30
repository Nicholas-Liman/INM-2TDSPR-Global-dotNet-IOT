using AshBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AshBoard.Application.Repositories
{
    public interface IArraySensorRepository
    {
        Task<List<ArraySensor>> GetAllAsync();
        Task<ArraySensor> GetByIdAsync(int id);
        Task CreateAsync(ArraySensor arraySensor);
        Task UpdateAsync(ArraySensor arraySensor);
        Task DeleteAsync(int id);
    }
}
