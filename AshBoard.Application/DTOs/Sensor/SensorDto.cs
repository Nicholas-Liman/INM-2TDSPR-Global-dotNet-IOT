namespace AshBoard.Application.DTOs.Sensor
{
    public class SensorDto
    {
        public string Id { get; set; } = string.Empty;
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Leitura em tempo real (podem ser nulos inicialmente)
        public double? Temperatura { get; set; }
        public double? NivelCO2 { get; set; }
        public string? DirecaoVento { get; set; }
        public DateTime? DataUltimaLeitura { get; set; }
        public int? ArraySensorId { get; set; }
    }
}
