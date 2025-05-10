using Common.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class Customer : DomainModelBase
    {
        public Customer()
        {
            this.TrustValue = TrustValueEnum.High;
        }


        [Required]
        [MaxLength(10000)]
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
        public string Email { get; set; }

        public TrustValueEnum TrustValue { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}
