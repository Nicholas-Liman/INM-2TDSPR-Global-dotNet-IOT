namespace AshBoard.Application.DTOs.ArraySensor
{
    public class CreateArraySensorDto
    {
        public string NomeLocal { get; set; } = string.Empty;

        public List<string> SensorIds { get; set; } = new();
    }
}
