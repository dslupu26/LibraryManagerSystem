using Business.Dtos;
using Business.Generic;
using DomainModels;

namespace Bussiness
{
    public interface IBookManager : IGenericDtoManager<Book, BookDto>
    {
        IEnumerable<BookDto> GetAllBooks();

        public BookDto InitializeNewItem();
    }
}
