namespace AshBoard.Domain.Entities
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // Ex: Sensor A1
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Relacionamento com ArraySensor (opcional)
        public int? ArraySensorId { get; set; }
        public ArraySensor? ArraySensor { get; set; }

        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
