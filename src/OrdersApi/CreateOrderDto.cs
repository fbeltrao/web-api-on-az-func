using System.ComponentModel.DataAnnotations;

namespace OrdersApi
{
    public class CreateOrderDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }
    }
}