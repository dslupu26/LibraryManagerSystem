using Common.DomainModels;

namespace DomainModels
{
    public class Rent : DomainModelBase 
    {
        public Book Book { get; set; }

        public int BookId { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}
