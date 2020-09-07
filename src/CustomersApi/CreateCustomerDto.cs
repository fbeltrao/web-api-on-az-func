using System.ComponentModel.DataAnnotations;

namespace CustomersApi
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int Age { get; set; }
    }
}