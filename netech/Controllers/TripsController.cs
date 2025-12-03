using Microsoft.AspNetCore.Mvc;
using netech.Api.ViewModels;
using netech.Core.Entities;
using netech.Core.Exceptions;
using netech.Core.Interfaces;
using netech.Infrastructure.Services;

namespace netech.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripRepository _repository;
        private readonly ICarbonCalculatorService _calculator;

        public TripsController(ITripRepository repository, ICarbonCalculatorService calculator)
        {
            _repository = repository;
            _calculator = calculator;
        }

        [HttpPost]
        public async Task<IActionResult> LogTrip([FromBody] LogTripRequest request)
        {
            // 1. Obter Fatores (Dados Mestres)
            var modeFactor = await _repository.GetFactorByIdAsync(request.TransportModeId);
            var baselineFactor = await _repository.GetBaselineFactorAsync();

            if (modeFactor == null || baselineFactor == null)
                return BadRequest("Modal de transporte inválido ou base não configurada.");

            // 2. Calcular a "Física" (Domínio)
            var savings = _calculator.CalculateSavings(request.DistanceMeters, modeFactor, baselineFactor);

            // 3. Criar a Entidade (Model)
            // Simulação de UserID fixo (em produção viria do JWT)
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var trip = new Trip(userId, request.TransportModeId, request.DistanceMeters, request.StartDateTime, request.EndDateTime);
            trip.SetCarbonSavings(savings);

            // 4. Persistir (Infra)
            await _repository.AddAsync(trip);

            // 5. Retornar ViewModel (Response)
            return CreatedAtAction(nameof(GetHistory), new { }, new TripResponse
            {
                Id = trip.Id,
                Co2SavedGrams = trip.Co2SavedGrams,
                DistanceKm = trip.DistanceMeters / 1000m,
                Mode = modeFactor.ModeName,
                Date = trip.StartDateTime
            });
        }

        // Endpoint com Paginação Keyset (Cursor)
        [HttpGet("history")]
        public async Task<ActionResult<List<TripResponse>>> GetHistory(
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTimeOffset? lastDate = null,
            [FromQuery] Guid? lastId = null)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var trips = await _repository.GetTripsByUserAsync(userId, pageSize, lastDate, lastId);

            // Mapeamento Manual (Model -> ViewModel)
            var response = trips.Select(t => new TripResponse
            {
                Id = t.Id,
                Co2SavedGrams = t.Co2SavedGrams,
                DistanceKm = t.DistanceMeters / 1000m,
                Date = t.StartDateTime,
                Mode = "Carregado via ID " + t.TransportModeId // Simplificado para demo
            }).ToList();

            return Ok(response);
        }
    }
}
