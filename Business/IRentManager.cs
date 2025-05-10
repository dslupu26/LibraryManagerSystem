using Business.Dtos;
using Business.Generic;
using DomainModels;

namespace Bussiness
{
    public interface IRentManager : IGenericDtoManager<Rent, RentDto>
    {
        IEnumerable<RentDto> GetAllRents();
        public IEnumerable<RentDto> GetAllLateRents();

        void RentBook(int customerCode, string isbn);

        void ReturnBook(int rentId);
    }
}
