namespace AshBoard.Application.DTOs.Sensor
{
    public class UpdateSensorDto
    {
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Campos de leitura
        public double? Temperatura { get; set; }
        public double? NivelCO2 { get; set; }
        public string? DirecaoVento { get; set; }
        public DateTime? DataUltimaLeitura { get; set; }

        public int? ArraySensorId { get; set; }
    }
}
