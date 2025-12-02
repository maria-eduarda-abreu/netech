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

        // Implementação da Paginação via Cursor (Keyset)
        public async Task<List<Trip>> GetTripsByUserAsync(Guid userId, int pageSize, DateTimeOffset? lastDate, Guid? lastId)
        {
            var query = _context.Trips
                .AsNoTracking() // Performance: não precisamos rastrear mudanças apenas para leitura
                .Where(t => t.UserId == userId);

            // Se o cliente enviou o cursor (última data e último ID que viu),
            // buscamos apenas os registros MAIS ANTIGOS que aquele ponto.
            if (lastDate.HasValue && lastId.HasValue)
            {
                query = query.Where(t =>
                    t.StartDateTime < lastDate.Value ||
                    (t.StartDateTime == lastDate.Value && t.Id.CompareTo(lastId.Value) < 0));
            }

            return await query
                .OrderByDescending(t => t.StartDateTime) // Do mais recente para o antigo
                .ThenByDescending(t => t.Id)           // Desempate determinístico
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
