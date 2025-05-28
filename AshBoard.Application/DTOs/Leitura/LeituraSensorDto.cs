namespace AshBoard.Application.DTOs.Leitura
{
    public class LeituraSensorDto
    {
        public string SensorId { get; set; } = string.Empty;
        public double Temperatura { get; set; }
        public double NivelCO2 { get; set; }
        public string DirecaoVento { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
    }
}
