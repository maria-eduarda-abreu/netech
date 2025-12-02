using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using netech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace netech.Core.Interfaces
{
    public interface ITripRepository
    {
        Task AddAsync(Trip trip);
        Task<CarbonFactor?> GetFactorByIdAsync(int id);
        Task<CarbonFactor?> GetBaselineFactorAsync();

        // Assinatura da paginação Keyset
        Task<List<Trip>> GetTripsByUserAsync(Guid userId, int pageSize, DateTimeOffset? lastDate, Guid? lastId);
    }
}
