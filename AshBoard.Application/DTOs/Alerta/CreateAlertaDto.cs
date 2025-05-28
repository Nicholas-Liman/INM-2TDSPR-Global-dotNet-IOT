namespace AshBoard.Application.DTOs.Alerta
{
    public class CreateAlertaDto
    {
        public DateTime DataHoraColeta { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde";
        public bool IncendioProximo { get; set; }
        public int SensorId { get; set; }
    }
}
