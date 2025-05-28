using AshBoard.Application.DTOs.Sensor;

namespace AshBoard.Application.Interfaces
{
    public interface ISensorService
    {
        Task<List<SensorDto>> GetAllAsync();
        Task<SensorDto?> GetByIdAsync(string id);
        Task<SensorDto> CreateAsync(CreateSensorDto dto);
        Task<bool> UpdateAsync(string id, UpdateSensorDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
