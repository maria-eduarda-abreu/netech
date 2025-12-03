namespace netech.Api.ViewModels
{
    public class TripResponse
    {
        public Guid Id { get; set; }
        public string Mode { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public decimal Co2SavedGrams { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}