using AshBoard.Application.DTOs.Sensor;

namespace AshBoard.Application.DTOs.ArraySensor
{
    public class ArraySensorDto
    {
        public int Id { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public List<SensorDto> Sensores { get; set; } = new();
    }
}
