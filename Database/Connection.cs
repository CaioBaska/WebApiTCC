using API_TCC.Model;
using API_TCC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API_TCC.Database
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<UsuarioModel> UsuarioModel { get; set; }
        public DbSet<MonitoramentoModel> MonitoramentoModel { get; set; }
        public DbSet<PlantasModel> PlantasModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(_configuration.GetConnectionString("OracleConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TCC");
            modelBuilder.Entity<UsuarioModel>().ToTable("USUARIOS");
            modelBuilder.Entity<MonitoramentoModel>().ToTable("MONITORAMENTO");
            modelBuilder.Entity<PlantasModel>().ToTable("PLANTAS");
        }
    }
}
