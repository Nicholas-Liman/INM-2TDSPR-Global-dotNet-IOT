namespace AshBoard.Domain.Entities
{
    public class ArraySensor
    {
        public int Id { get; set; }
        public string NomeLocal { get; set; } = string.Empty; // Exemplo -> Bosque das corujas

        public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();
    }
}
