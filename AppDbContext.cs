using Microsoft.EntityFrameworkCore;

namespace proiect_practica
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produs> Produse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produs>().ToTable("produs", schema: "dbo");
        }
    }
}
