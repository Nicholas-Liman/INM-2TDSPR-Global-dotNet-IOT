namespace SensorEmulator.Models
{
    public class SensorDto
    {
        public string Id { get; set; } = string.Empty;

        public string NomeLocal { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
