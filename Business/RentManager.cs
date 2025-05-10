using System.Linq.Expressions;
using AutoMapper;
using Business.Dtos;
using Bussiness;
using Bussiness.Common;
using Bussiness.Generic;
using Common;
using Common.Repositories;
using DomainModels;

namespace Business
{

namespace Business
    {
        public class RentManager : GenericDtoManager<Rent, RentDto>, IRentManager
        {

            private ICurrentUserProvider currentUserProvider;
            private readonly IRepositoryFactory repositoryFactory;
            private readonly IUnitOfWorkFactory unitOfWorkFactory;
            private readonly IMapper mapper;
            public RentManager(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory, IMapper mapper) : base(currentUserProvider, unitOfWorkFactory, repositoryFactory, mapper)
            {
                ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
                ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));
                ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

                this.currentUserProvider = currentUserProvider;
                this.repositoryFactory = repositoryFactory;
                this.unitOfWorkFactory = unitOfWorkFactory;
                this.mapper = mapper;
            }

            public IEnumerable<RentDto> GetAllRents()
            {
                var result = this.GetAllItems(it => it.Customer, it => it.Book);
                return result;
            }

            public IEnumerable<RentDto> GetAllLateRents()
            {
                var condition = new List<Expression<Func<Rent, bool>>> { it => it.ReturnDate < DateTime.Now.Date.AddDays(1)};
                var result = this.GetAllItems(condition, it => it.Customer, it => it.Book);
                return result;
            }

            public void RentBook(int customerCode, string isbn)
            {
                ArgumentNullException.ThrowIfNull(customerCode, nameof(customerCode));
                ArgumentNullException.ThrowIfNull(isbn, nameof(isbn));

                using (var unitOfWork = this.unitOfWorkFactory.GetNew())
                {
                    var bookRepository = repositoryFactory.GetNew<Book>(unitOfWork);
                    var book = bookRepository.GetAll(it => it.ISBN == isbn).FirstOrDefault();
                    if (book == null)
                    {
                        throw new BusinessException($"No book with this ISBN: {isbn}");
                    }

                    if (book.AvailableQuantity == 0)
                    {
                        throw new BusinessException($"No item for this rent for rent.");
                    }

                    var customerRepository = repositoryFactory.GetNew<Customer>(unitOfWork);
                    var customer = customerRepository.GetAll(it => it.CustomerCode == customerCode).FirstOrDefault();
                    if (customer == null)
                    {
                        throw new BusinessException($"No customer with code: {customerCode}");
                    }

                    if (customer.TrustValue == TrustValueEnum.Low)
                    {
                        throw new BusinessException($"The customer with code: {customerCode} is not allowed to rent.");
                    }
                    unitOfWork.BeginTransaction();

                    var rentRepository = repositoryFactory.GetNew<Rent>(unitOfWork);

                    var customerRents = rentRepository.GetAll(it => it.CustomerId == customer.Id);
                    if (customerRents.Count() >= 3)
                    {
                        throw new BusinessException($"No more than 3 books are allowed to be rent.");
                    }

                    if (customerRents.Any(it => it.ReturnDate < DateTime.Now.Date.AddDays(1)))
                    {
                        throw new BusinessException($"You can not rent a new rent because you are late with other one.");
                    }

                    var rent = new Rent();
                    rent.BookId = book.Id;
                    rent.CustomerId = customer.Id;
                    rent.ReturnDate = DateTime.Now.Date.AddDays(book.NumberOfReturnDays);
                    rentRepository.Add(rent);

                    book.AvailableQuantity--;
                    bookRepository.Update(book);

                    unitOfWork.SaveChanges();
                    unitOfWork.CommitTransaction();
                }
            }

            private void UpdateCustomerLevel(IUnitOfWork unitOfWork, int customerId)
            {
                var customerRepository = repositoryFactory.GetNew<Customer>(unitOfWork);
                var customer = customerRepository.Get(customerId);
                bool wasChange = false;
                if (customer.TrustValue == TrustValueEnum.High)
                {
                    customer.TrustValue = TrustValueEnum.Medium;
                    wasChange = true;
                }
                else if (customer.TrustValue == TrustValueEnum.Medium)
                {
                    customer.TrustValue = TrustValueEnum.Low;
                    wasChange = true;
                }
                if (wasChange)
                {
                    customerRepository.Update(customer);
                }
            }

            public void ReturnBook(int rentId)
            {
                using (var unitOfWork = this.unitOfWorkFactory.GetNew())
                {
                    var rentRepository = repositoryFactory.GetNew<Rent>(unitOfWork);
                    var rent = rentRepository.Get(rentId);
                    if (rent == null)
                    {
                        throw new Exception($"No rent with id : {rentId}.");
                    }

                    unitOfWork.BeginTransaction();

                    var bookRepository = repositoryFactory.GetNew<Book>(unitOfWork);
                    var book = bookRepository.Get(rent.BookId);
                    book.AvailableQuantity++;
                    bookRepository.Update(book);

                    if (rent.ReturnDate < DateTime.Now.Date.AddDays(1))
                    {
                        this.UpdateCustomerLevel(unitOfWork, rent.CustomerId);
                    }

                    rentRepository.Delete(rent);
                    unitOfWork.SaveChanges();

                    unitOfWork.CommitTransaction();
                }
            }
        }
    }
}
