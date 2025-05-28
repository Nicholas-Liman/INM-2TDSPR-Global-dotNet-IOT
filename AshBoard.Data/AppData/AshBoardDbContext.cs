using Microsoft.EntityFrameworkCore;
using AshBoard.Domain.Entities;

namespace AshBoard.Data
{
    public class AshBoardDbContext : DbContext
    {
        public AshBoardDbContext(DbContextOptions<AshBoardDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sensor> Sensores { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<ArraySensor> ArraysDeSensores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais de entidades, se necessário
        }
    }
}
