using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using netech.Core.Entities;

namespace netech.Core.Interfaces
{
    public interface ICarbonCalculatorService
    {
        /// <summary>
        /// Calcula a economia de CO2 baseada na distância e no modal escolhido.
        /// Baseado na fórmula: S = d * (Fbase - Fmodal)
        /// </summary>
        decimal CalculateSavings(decimal distanceMeters, CarbonFactor transportMode, CarbonFactor baselineMode);
    }
}
