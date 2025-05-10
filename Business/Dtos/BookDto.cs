using System.ComponentModel.DataAnnotations;

namespace Business.Dtos
{
    public class BookDto : BaseDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(17)]
        public string ISBN { get; set; }

        public int MaxQuantity { get; set; }

        public int AvailableQuantity { get; set; }

        public int NumberOfReturnDays { get; set; }
    }
}
