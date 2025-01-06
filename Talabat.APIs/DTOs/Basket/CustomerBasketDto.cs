using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs.Basket
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> BasketProducts { get; set; }
    }
}
