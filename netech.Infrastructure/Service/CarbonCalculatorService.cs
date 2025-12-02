using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netech.Core.Entities;
using netech.Core.Interfaces;

namespace netech.Infrastructure.Services
{
    public class CarbonCalculatorService : ICarbonCalculatorService
    {
        public decimal CalculateSavings(decimal distanceMeters, CarbonFactor transportMode, CarbonFactor baselineMode)
        {
            // Converte metros para km
            decimal distanceKm = distanceMeters / 1000m;

            // Fórmula: (FatorBase - FatorModal) * Distância
            // Se o modal for mais poluente que a base (ex: Jato Privado), o resultado será negativo.
            decimal savingsPerKm = baselineMode.EmissionPerKm - transportMode.EmissionPerKm;

            decimal totalSavings = savingsPerKm * distanceKm;

            // Regra de Negócio: Não permitimos "poupança negativa" (gerar poluição não dá pontos)
            // A menos que o requisito mude para penalizar o usuário.
            return totalSavings > 0 ? totalSavings : 0;
        }
    }
}