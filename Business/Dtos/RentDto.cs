namespace Business.Dtos
{
    public class RentDto : BaseDto
    {
        public string CustomerName { get; set; }

        public string CustomerCode { get; set; }

        public string BookTitle { get; set; }

        public string ISBN { get; set; }

        public string ReturnDate { get; set; }
    }
}
