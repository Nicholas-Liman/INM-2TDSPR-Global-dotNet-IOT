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
        public DbSet<ArraySensor> ArraySensores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Impede geração automática do ID do Sensor
            modelBuilder.Entity<Sensor>()
                .Property(s => s.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Sensor>()
                .HasOne(s => s.ArraySensor)
                .WithMany(a => a.Sensores)
                .HasForeignKey(s => s.ArraySensorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Alerta>()
                .HasOne(a => a.Sensor)
                .WithMany(s => s.Alertas)
                .HasForeignKey(a => a.SensorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
