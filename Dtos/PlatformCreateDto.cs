using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public record PlatformCreateDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Publisher { get; init; }

        [Required]
        public string Cost { get; init; }
    }
}