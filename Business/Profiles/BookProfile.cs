using DomainModels;
using AutoMapper;
using Business.Dtos;

namespace Bussiness.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            this.CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
