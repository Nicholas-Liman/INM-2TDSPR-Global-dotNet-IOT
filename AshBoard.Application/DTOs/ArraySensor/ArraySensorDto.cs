namespace AshBoard.Application.DTOs.ArraySensor
{
    public class ArraySensorDto
    {
        public int Id { get; set; }
        public string NomeLocal { get; set; } = string.Empty;

        public List<string> SensoresIds { get; set; } = new();
    }
}
