using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netech.Core.Entities;

namespace netech.Core.Interfaces
{
    public interface ITripRepository
    {
        Task AddAsync(Trip trip);

        // Verifique se estes dois métodos existem:
        Task<CarbonFactor?> GetFactorByIdAsync(int id);
        Task<CarbonFactor?> GetBaselineFactorAsync();

        // Verifique se a assinatura da paginação está igual a esta:
        Task<List<Trip>> GetTripsByUserAsync(Guid userId, int pageSize, DateTimeOffset? lastDate, Guid? lastId);
    }
}
