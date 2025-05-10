using Business.Dtos;
using DomainModels;
using AutoMapper;

namespace Business.Profiles
{
    public class RentProfile : Profile
    {
        public RentProfile()
        {
            this.CreateMap<Rent, RentDto>()
                .ForMember(it => it.ReturnDate, opt => opt.MapFrom(it => it.ReturnDate.ToString("dd/MM/yyyy")))
                .ForMember(it => it.CustomerName, opt => opt.MapFrom(it => it.Customer.Name))
                .ForMember(it => it.CustomerCode, opt => opt.MapFrom(it => it.Customer.CustomerCode))
                .ForMember(it => it.BookTitle, opt => opt.MapFrom(it => it.Book.Title))
                .ForMember(it => it.ISBN, opt => opt.MapFrom(it => it.Book.ISBN))
                .ReverseMap();
        }
    }
}
