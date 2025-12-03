using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netech.Core.Entities
{
    public class Trip
    {
        // Construtor: Força a validação das regras básicas ao criar o objeto
        public Trip(Guid userId, int transportModeId, decimal distanceMeters, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
        {
            if (distanceMeters <= 0)
                throw new ArgumentException("A distância deve ser maior que zero.", nameof(distanceMeters));

            if (endDateTime <= startDateTime)
                throw new ArgumentException("A data de fim deve ser posterior à data de início.", nameof(endDateTime));

            Id = Guid.NewGuid(); // Gera um novo ID automaticamente
            UserId = userId;
            TransportModeId = transportModeId;
            DistanceMeters = distanceMeters;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;

            // Co2SavedGrams começa zerado e deve ser definido via SetCarbonSavings
        }

        // Propriedades com 'private set' para garantir imutabilidade externa
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public int TransportModeId { get; private set; }

        // Uso de decimal para alta precisão em cálculos ESG
        public decimal DistanceMeters { get; private set; }
        public decimal Co2SavedGrams { get; private set; }

        // DateTimeOffset para garantir o fuso horário correto
        public DateTimeOffset StartDateTime { get; private set; }
        public DateTimeOffset EndDateTime { get; private set; }

        // Método para "gravar" o cálculo de carbono de forma definitiva (Snapshot)
        public void SetCarbonSavings(decimal savedGrams)
        {
            // Regra de segurança: impede a alteração de dados históricos
            if (Co2SavedGrams != 0)
                throw new InvalidOperationException("A economia de CO2 já foi registrada para esta viagem.");

            Co2SavedGrams = savedGrams;
        }
    }
}
