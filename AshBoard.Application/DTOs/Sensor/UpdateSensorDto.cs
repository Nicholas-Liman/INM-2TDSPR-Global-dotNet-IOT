namespace AshBoard.Application.DTOs.Sensor
{
    public class UpdateSensorDto
    {
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Temperatura { get; set; }
        public double? NivelCO2 { get; set; }
        public double? DirecaoVento { get; set; }
        public int? ArraySensorId { get; set; }
    }
}
