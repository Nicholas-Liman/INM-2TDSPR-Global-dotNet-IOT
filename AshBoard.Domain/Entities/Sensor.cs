namespace AshBoard.Domain.Entities
{
    public class Sensor
    {
        public string Id { get; set; } = string.Empty;           // Ex: "A3"
        public string NomeLocal { get; set; } = string.Empty;    // Ex: "Córrego das Corujas"

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double? Temperatura { get; set; }     // Populado via simulação
        public double? NivelCO2 { get; set; }        // Populado via simulação
        public double? DirecaoVento { get; set; }    // Populado via simulação

        public int? ArraySensorId { get; set; }      // Caso o sensor seja adicionado a um Array de Sensores
        public ArraySensor? ArraySensor { get; set; }

        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    }
}
