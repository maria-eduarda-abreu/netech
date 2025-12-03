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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configuração da Entidade Trip ---
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Precisão Decimal para evitar erros de arredondamento (Item 3.1 do relatório)
                entity.Property(e => e.DistanceMeters).HasPrecision(18, 2);
                entity.Property(e => e.Co2SavedGrams).HasPrecision(18, 4);

                // Índices compostos para performance máxima na Paginação Keyset
                // O banco precisa encontrar rapidamente registros baseados em Data E ID.
                entity.HasIndex(e => new { e.StartDateTime, e.Id });
                entity.HasIndex(e => e.UserId);
            });

            // --- Configuração da Entidade CarbonFactor ---
            modelBuilder.Entity<CarbonFactor>(entity =>
            {
                entity.Property(e => e.EmissionPerKm).HasPrecision(18, 4);

                // Seed Inicial (Dados da Tabela 1.2 do relatório)
                entity.HasData(
                    new CarbonFactor { Id = 1, ModeName = "Carro Gasolina (Base)", EmissionPerKm = 271.0m, IsBaseline = true },
                    new CarbonFactor { Id = 2, ModeName = "E-Bike", EmissionPerKm = 9.0m, IsBaseline = false },
                    new CarbonFactor { Id = 3, ModeName = "Bicicleta Mecânica", EmissionPerKm = 5.0m, IsBaseline = false },
                    new CarbonFactor { Id = 4, ModeName = "Ônibus (Médio)", EmissionPerKm = 100.0m, IsBaseline = false },
                    new CarbonFactor { Id = 5, ModeName = "Caminhada", EmissionPerKm = 0.0m, IsBaseline = false }
                );
            });
        }
    }
}