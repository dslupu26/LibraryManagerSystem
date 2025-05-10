using System.ComponentModel.DataAnnotations;

namespace Business.Dtos
{
    public class CustomerDto : BaseDto
    {

        [Required]
        [MaxLength(10000)]
        [MinLength(1000)]
        public int CustomerCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Emailul nu este valid.")]
        public string Email { get; set; }

        [Required]
        public string TrustValue { get; set; }
    }
}
