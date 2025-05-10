using System.Linq.Expressions;
using AutoMapper;
using Business.Dtos;
using Bussiness.Common;
using Bussiness.Generic;
using Common;
using Common.Repositories;
using DomainModels;
using System.Text.RegularExpressions;

namespace Business
{
    public class CustomerManager : GenericDtoManager<Customer, CustomerDto>, ICustomerManager
    {
        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMapper mapper;
        public CustomerManager(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory, IMapper mapper) : base(currentUserProvider, unitOfWorkFactory, repositoryFactory, mapper)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mapper = mapper;
        }

        private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CustomerDto GetCustomer(int id)
        {
            var list = this.GetAllItems(new List<Expression<Func<Customer, bool>>> { it => it.Id == id });
            return list.FirstOrDefault();
        }

        public IEnumerable<CustomerDto> GetAllCustomers()
        {
            var result = this.GetAllItems();
            return result;
        }

        private void Validate(Customer customer)
        {
            if(!EmailRegex.IsMatch(customer.Email))
            {
                throw new BusinessException("The email provided does not support a valid format!");
            }

            if (customer.CustomerCode < 1000 || customer.CustomerCode > 10000) 
            {
                throw new BusinessException("CustomerCode >=1000 AND CustomerCode <=10000");
            }
        }

        protected override void BeforeAdd(IUnitOfWork unitOfWork, Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(customer.Email, nameof(customer.Email));
            ArgumentNullException.ThrowIfNull(customer.CustomerCode, nameof(customer.CustomerCode));

            base.BeforeAdd(unitOfWork, customer);

            this.Validate(customer);

            var existingCustomerEmail = this.Get(customer.Email);
            if (existingCustomerEmail != null)
            {
                throw new BusinessException($"There already exists a customer with that email: {customer.Email}.");
            }

            var existingCustomerCode = this.Get(customer.CustomerCode);
            if (existingCustomerCode != null)
            {
                throw new BusinessException($"There already exists a customer with that code: {customer.CustomerCode}.");
            }
        }

        protected override void BeforeUpdate(IUnitOfWork unitOfWork, Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(customer.Email, nameof(customer.Email));
            ArgumentNullException.ThrowIfNull(customer.CustomerCode, nameof(customer.CustomerCode));

            base.BeforeUpdate(unitOfWork, customer);

            this.Validate(customer);

            var condition1 = new List<Expression<Func<Customer, bool>>> { it => it.Id != customer.Id && it.Email == customer.Email };

            var existingCustomerEmail = this.GetAllItems(condition1).FirstOrDefault();
            if (existingCustomerEmail != null)
            {
                throw new BusinessException($"There already exists a customer with that email: {customer.Email}.");
            }

            var condition2 = new List<Expression<Func<Customer, bool>>> { it => it.Id != customer.Id && it.CustomerCode == customer.CustomerCode };

            var existingCustomerCode = this.GetAllItems(condition2).FirstOrDefault();
            if (existingCustomerCode != null)
            {
                throw new BusinessException($"There already exists a customer with that code: {customer.CustomerCode}.");
            }
        }

        protected override void BeforeDelete(IUnitOfWork unitOfWork, Customer customer)
        {
            base.BeforeDelete(unitOfWork, customer);
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            if (this.GetById(customer.Id, unitOfWork) == null)
            {
                throw new BusinessException("Customer not found!");
            }

            var repository = repositoryFactory.GetNew<Rent>(unitOfWork);
            if (repository.Any(it => it.CustomerId == customer.Id))
            {
                throw new BusinessException("You cannot delete this customer because it is renting a book.");
            }
        }

        private CustomerDto Get(string email)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<Customer>(unitOfWork);
                var customer = repo.GetAll(it => it.Email == email).FirstOrDefault();
                return customer == null ? null : mapper.Map<CustomerDto>(customer);
            }
        }

        private CustomerDto Get(int customercode)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<Customer>(unitOfWork);
                var customer = repo.GetAll(it => it.CustomerCode == customercode).FirstOrDefault();
                return customer == null ? null : mapper.Map<CustomerDto>(customer);
            }
        }

        public CustomerDto InitializeNewItem()
        {
            var customer = new Customer();
            customer.TrustValue = TrustValueEnum.High;
            var res = mapper.Map<CustomerDto>(customer);            

            return res;
        }
    }
}
