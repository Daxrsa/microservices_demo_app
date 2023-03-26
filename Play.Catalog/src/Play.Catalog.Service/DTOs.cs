using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service
{
    public class DTOs
    {
        public record ItemDTO
        (
            Guid Id,
            string Name,
            string Description,
            decimal Price,
            DateTimeOffset CreatedDate
        );

        public record CreateItemDTO
        (
            Guid Id,
            [Required]
            string Name,
            string Description,
            [Range(0, 1000)]
            decimal Price
        );

        public record UpdateItemDTO
        (
            Guid Id,
            [Required]
            string Name,
            string Description,
            [Range(0, 1000)]
            decimal Price
        );
    }
}