using Business.Dtos;
using Business.Generic;
using DomainModels;

namespace Business
{
    public interface ICustomerManager : IGenericDtoManager<Customer, CustomerDto>
    {
        IEnumerable<CustomerDto> GetAllCustomers();

        public CustomerDto InitializeNewItem();
    }
}
