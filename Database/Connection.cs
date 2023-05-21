using API_TCC.Model;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Database
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioModel> LoginModel { get; set; }
        public DbSet<MonitoramentoModel> MonitoramentoModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TCC"); // adiciona o schema padrão para todas as tabelas
            modelBuilder.Entity<UsuarioModel>().ToTable("USUARIOS"); // define a tabela para a entidade Usuario
            modelBuilder.Entity<MonitoramentoModel>().ToTable("MONITORAMENTO").HasNoKey(); // define a tabela para a entidade MonitoramentoModel
        }
    }
}
