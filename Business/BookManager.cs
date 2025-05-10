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
    public class BookManager : GenericDtoManager<Book, BookDto>, IBookManager
    {

        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMapper mapper;
        public BookManager(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory, IMapper mapper) : base(currentUserProvider, unitOfWorkFactory, repositoryFactory, mapper)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mapper = mapper;
        }

        public BookDto GetBook(int id)
        {
            var list = this.GetAllItems(new List<Expression<Func<Book, bool>>> { it => it.Id == id });
            return list.FirstOrDefault();
        }

        public IEnumerable<BookDto> GetAllBooks()
        {
            var result = this.GetAllItems();
            return result;
        }

        private void Validate(Book book)
        {                        
               if (book.MaxQuantity <= 0)
            {
                throw new BusinessException($"MaxQuantity > 0");
            }

            if (book.AvailableQuantity <= 0)
            {
                throw new BusinessException($"AvailableQuantity > 0");
            }

            if (book.AvailableQuantity > book.MaxQuantity)
            {
                throw new BusinessException($"AvailableQuantity <= MaxQuantity");
            }

            if (book.NumberOfReturnDays <= 0 && book.NumberOfReturnDays > 31)
            {
                throw new BusinessException($"NumberOfReturnDays > 0 && NumberOfReturnDays <=31");
            }

            if (book.ISBN.Length > 20 || book.ISBN.Length < 17)
            {
                throw new BusinessException($"ISBN.Length <= 20 SI ISBN.Length >= 17");
            }
        }

        protected override void BeforeAdd(IUnitOfWork unitOfWork, Book book)
        {
            ArgumentNullException.ThrowIfNull(book, nameof(book));
            ArgumentNullException.ThrowIfNull(book.ISBN, nameof(book.ISBN));

            base.BeforeAdd(unitOfWork, book);

            this.Validate(book);

            var existingBook = this.Get(book.ISBN);
            if (existingBook != null)
            {
                throw new BusinessException($"There exists another book with that ISBN code: {book.ISBN}.");
            }
        }

        protected override void BeforeUpdate(IUnitOfWork unitOfWork, Book model)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(model.ISBN, nameof(model.ISBN));

            base.BeforeUpdate(unitOfWork, model);

            this.Validate(model);

            var condition = new List<Expression<Func<Book, bool>>> { it => it.Id != model.Id  && it.ISBN == model.ISBN };

            var existingBook = this.GetAllItems(condition).FirstOrDefault();
            if (existingBook != null)
            {
                throw new BusinessException($"There exists another book with that ISBN code: {model.ISBN}.");
            }
        }

        protected override void BeforeDelete(IUnitOfWork unitOfWork, Book book)
        {
            base.BeforeDelete(unitOfWork, book);
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            if (this.GetById(book.Id, unitOfWork) == null)
            {
                throw new BusinessException("Book not found!");
            }
            
            var repository = repositoryFactory.GetNew<Rent>(unitOfWork);            
            if (repository.Any(it => it.BookId == book.Id))
            {
                throw new BusinessException("You cannot delete this book because it is being rented.");
            }            
        }

        private BookDto Get(string isbn)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<Book>(unitOfWork);
                var book = repo.GetAll(it => it.ISBN == isbn).FirstOrDefault();
                return book == null ? null : mapper.Map<BookDto>(book);
            }
        }

        public IEnumerable<BookDto> SearchBooks(string title, string author, string genre, string isbn)
        {
            var criteria = new List<Expression<Func<Book, bool>>>();

            var allBooks = GetAllBooks();
            var query = allBooks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                criteria.Add(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                criteria.Add(b => b.Author.Contains(author));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                criteria.Add(b => b.Genre.Contains(genre));
            }

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                criteria.Add(b => b.ISBN.Contains(isbn));
            }

            var books = this.GetAllItems(criteria);

            return books;
        }

        public BookDto InitializeNewItem()
        {
            var book = new Book();
            var res = mapper.Map<BookDto>(book);
            res.NumberOfReturnDays = 15;
            res.MaxQuantity = 1;
            res.AvailableQuantity = 1;

            return res;
        }
    }
}
