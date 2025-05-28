namespace AshBoard.Domain.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Temperatura { get; set; }
        public double NivelCO2 { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? ArraySensorId { get; set; } // Relacionamento com ArraySenor
        public ArraySensor? ArraySensor { get; set; }

        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
