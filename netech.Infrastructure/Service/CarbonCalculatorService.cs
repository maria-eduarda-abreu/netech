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
            // Converter metros para km, pois os fatores estão em g/km
            decimal distanceKm = distanceMeters / 1000m;

            // Fórmula: (FatorBase - FatorModal) * Distância
            // Ex: (271g - 9g) * 10km = 2620g de economia.
            decimal savingsPerKm = baselineMode.EmissionPerKm - transportMode.EmissionPerKm;

            decimal totalSavings = savingsPerKm * distanceKm;

            // Regra de Negócio: Não retornamos economia negativa.
            return totalSavings > 0 ? totalSavings : 0;
        }
    }
}