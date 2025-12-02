using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netech.Core.Entities;

namespace netech.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<CarbonFactor> CarbonFactors { get; set; }
        // public DbSet<User> Users { get; set; } // Descomente quando criar a entidade User

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da Entidade Trip
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Precisão Decimal para evitar erros de arredondamento em dados financeiros/ESG
                entity.Property(e => e.DistanceMeters).HasPrecision(18, 2);
                entity.Property(e => e.Co2SavedGrams).HasPrecision(18, 4);

                // Índices para performance da Paginação Keyset
                // O banco precisa achar rápido por Data e ID
                entity.HasIndex(e => new { e.StartDateTime, e.Id });
                entity.HasIndex(e => e.UserId);
            });

            // Configuração da Entidade CarbonFactor
            modelBuilder.Entity<CarbonFactor>(entity =>
            {
                entity.Property(e => e.EmissionPerKm).HasPrecision(18, 4);

                // Seed Inicial (Dados de Pesquisa)
                entity.HasData(
                    new CarbonFactor { Id = 1, ModeName = "Carro Gasolina (Base)", EmissionPerKm = 271.0m, IsBaseline = true },
                    new CarbonFactor { Id = 2, ModeName = "E-Bike", EmissionPerKm = 9.0m, IsBaseline = false },
                    new CarbonFactor { Id = 3, ModeName = "Bicicleta Mecânica", EmissionPerKm = 5.0m, IsBaseline = false }
                );
            });
        }
    }
}