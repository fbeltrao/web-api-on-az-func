﻿using System.ComponentModel.DataAnnotations;

namespace OrdersApi
{
    public class OrderDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string CustomerName { get; set; }
    }
}