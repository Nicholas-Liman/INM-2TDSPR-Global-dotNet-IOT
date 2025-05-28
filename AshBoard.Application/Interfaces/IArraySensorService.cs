using AshBoard.Application.DTOs.ArraySensor;

namespace AshBoard.Application.Interfaces
{
    public interface IArraySensorService
    {
        Task<List<ArraySensorDto>> GetAllAsync();
        Task<ArraySensorDto?> GetByIdAsync(int id);
        Task<ArraySensorDto> CreateAsync(CreateArraySensorDto dto);
        Task<bool> UpdateAsync(int id, UpdateArraySensorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
