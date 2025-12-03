using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netech.Core.Entities
{
    public class CarbonFactor
    {
        public int Id { get; set; }
        public string ModeName { get; set; } = string.Empty;

        // Fator de emissão em gramas de CO2e por KM
        public decimal EmissionPerKm { get; set; }

        // Se é o modal de referência (Base)
        public bool IsBaseline { get; set; }
    }
}