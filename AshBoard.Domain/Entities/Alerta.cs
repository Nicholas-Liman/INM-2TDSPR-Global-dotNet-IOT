namespace AshBoard.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public DateTime DataColeta { get; set; }
        public DateTime HoraColeta { get; set; }
        public string NomeLocal { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde"; // Verde -> Amarelo -> Vermelho

        public bool IncendioProximo { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}