using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RegisterUse> RegistersUse => Set<RegisterUse>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<State> States => Set<State>();
        public DbSet<TollStation> TollStations => Set<TollStation>();

        public void ApplyMigrations()
        {
            try
            {
                if (!Database.IsRelational())
                    return;

                Database.Migrate();
                Console.WriteLine("Migrations applied.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegisterUse>(entity =>
            {
                entity.ToTable("RegistersUse");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UsedAt)
                      .IsRequired();

                entity.Property(e => e.AmountPaid)
                      .IsRequired()
                      .HasColumnType("decimal(10,2)");

                entity.Property(e => e.VehicleType)
                      .IsRequired()
                      .HasConversion<int>();

                entity.HasOne(e => e.TollStation)
                      .WithMany()
                      .HasForeignKey(e => e.TollStationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasOne(e => e.State)
                      .WithMany()
                      .HasForeignKey(e => e.StateId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("States");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Uf)
                      .IsRequired()
                      .HasMaxLength(2)
                      .IsFixedLength();

                entity.HasData(
                    new State { Id = 1, Name = "Acre", Uf = "AC" },
                    new State { Id = 2, Name = "Alagoas", Uf = "AL" },
                    new State { Id = 3, Name = "Amapá", Uf = "AP" },
                    new State { Id = 4, Name = "Amazonas", Uf = "AM" },
                    new State { Id = 5, Name = "Bahia", Uf = "BA" },
                    new State { Id = 6, Name = "Ceará", Uf = "CE" },
                    new State { Id = 7, Name = "Distrito Federal", Uf = "DF" },
                    new State { Id = 8, Name = "Espírito Santo", Uf = "ES" },
                    new State { Id = 9, Name = "Goiás", Uf = "GO" },
                    new State { Id = 10, Name = "Maranhão", Uf = "MA" },
                    new State { Id = 11, Name = "Mato Grosso", Uf = "MT" },
                    new State { Id = 12, Name = "Mato Grosso do Sul", Uf = "MS" },
                    new State { Id = 13, Name = "Minas Gerais", Uf = "MG" },
                    new State { Id = 14, Name = "Pará", Uf = "PA" },
                    new State { Id = 15, Name = "Paraíba", Uf = "PB" },
                    new State { Id = 16, Name = "Paraná", Uf = "PR" },
                    new State { Id = 17, Name = "Pernambuco", Uf = "PE" },
                    new State { Id = 18, Name = "Piauí", Uf = "PI" },
                    new State { Id = 19, Name = "Rio de Janeiro", Uf = "RJ" },
                    new State { Id = 20, Name = "Rio Grande do Norte", Uf = "RN" },
                    new State { Id = 21, Name = "Rio Grande do Sul", Uf = "RS" },
                    new State { Id = 22, Name = "Rondônia", Uf = "RO" },
                    new State { Id = 23, Name = "Roraima", Uf = "RR" },
                    new State { Id = 24, Name = "Santa Catarina", Uf = "SC" },
                    new State { Id = 25, Name = "São Paulo", Uf = "SP" },
                    new State { Id = 26, Name = "Sergipe", Uf = "SE" },
                    new State { Id = 27, Name = "Tocantins", Uf = "TO" }
                );

            });

            modelBuilder.Entity<TollStation>(entity =>
            {
                entity.ToTable("TollStations");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasOne(e => e.City)
                      .WithMany()
                      .HasForeignKey(e => e.CityId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

        }
    }
}
