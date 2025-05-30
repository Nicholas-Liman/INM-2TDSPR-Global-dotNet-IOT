namespace AshBoard.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public DateTime DataHoraColeta { get; set; }
        public string NomeLocal { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde";
        public string SensorId { get; set; } = string.Empty;
        public Sensor Sensor { get; set; }
    }
}