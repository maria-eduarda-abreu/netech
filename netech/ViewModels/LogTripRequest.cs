using System.ComponentModel.DataAnnotations;

namespace netech.Api.ViewModels
{
    public class LogTripRequest
    {
        [Required]
        public int TransportModeId { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "A distância deve ser entre 10m e 10.000km")]
        public decimal DistanceMeters { get; set; }

        [Required]
        public DateTimeOffset StartDateTime { get; set; }

        [Required]
        public DateTimeOffset EndDateTime { get; set; }
    }
}