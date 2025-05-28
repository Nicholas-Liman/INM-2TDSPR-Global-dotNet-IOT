namespace AshBoard.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; } = null!;

        public DateTime DataColeta { get; set; }
        public DateTime HoraColeta { get; set; }

        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde"; // Verde -> Amarelo -> Vermelho
    }
}
