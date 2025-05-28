using Microsoft.EntityFrameworkCore;
using AshBoard.Domain.Entities;

namespace AshBoard.Data.AppData
{
    public class AshBoardDbContext : DbContext
    {
        public AshBoardDbContext(DbContextOptions<AshBoardDbContext> options)
            : base(options) { }

        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<ArraySensor> ArraysSensores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento Sensor -> ArraySensor
            modelBuilder.Entity<Sensor>()
                .HasOne(s => s.ArraySensor)
                .WithMany(a => a.Sensores)
                .HasForeignKey(s => s.ArraySensorId);

            modelBuilder.Entity<Alerta>()
                .HasOne(a => a.Sensor)
                .WithMany(s => s.Alertas)
                .HasForeignKey(a => a.SensorId);
        }
    }
}
