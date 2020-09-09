namespace OrdersApi
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }
    }
}