using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netech.Core.Entities;
using netech.Core.Interfaces;
using netech.Infrastructure.Data;

namespace netech.Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public TripRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<CarbonFactor?> GetFactorByIdAsync(int id)
        {
            return await _context.CarbonFactors.FindAsync(id);
        }

        public async Task<CarbonFactor?> GetBaselineFactorAsync()
        {
            return await _context.CarbonFactors.FirstOrDefaultAsync(f => f.IsBaseline);
        }

        // --- A Lógica Complexa de Keyset Pagination ---
        public async Task<List<Trip>> GetTripsByUserAsync(Guid userId, int pageSize, DateTimeOffset? lastDate, Guid? lastId)
        {
            var query = _context.Trips
                .AsNoTracking() // Otimização: Leitura rápida sem tracking
                .Where(t => t.UserId == userId);

            // Se temos um cursor (lastDate/lastId), filtramos apenas o que é mais antigo que ele.
            // Isso evita o "OFFSET" lento do SQL.
            if (lastDate.HasValue && lastId.HasValue)
            {
                query = query.Where(t =>
                    t.StartDateTime < lastDate.Value ||
                    (t.StartDateTime == lastDate.Value && t.Id.CompareTo(lastId.Value) < 0));
            }

            return await query
                .OrderByDescending(t => t.StartDateTime) // Ordenação obrigatória para o cursor funcionar
                .ThenByDescending(t => t.Id)           // Desempate obrigatório
                .Take(pageSize)
                .ToListAsync();
        }
    }
}