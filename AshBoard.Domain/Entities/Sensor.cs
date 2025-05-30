namespace AshBoard.Domain.Entities
{
    public class Sensor
    {
        public string Id { get; set; } = string.Empty;
        public string NomeLocal { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Populados via simulação
        public double? Temperatura { get; set; }
        public double? NivelCO2 { get; set; }
        public string? DirecaoVento { get; set; }
        public DateTime? DataUltimaLeitura { get; set; }

        public int? ArraySensorId { get; set; }
        public ArraySensor? ArraySensor { get; set; }

        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
