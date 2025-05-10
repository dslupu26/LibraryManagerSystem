using DomainModels;
using AutoMapper;
using Business.Dtos;

namespace Bussiness.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            this.CreateMap<Customer, CustomerDto>()
                .ForMember(it => it.TrustValue, opt => opt.MapFrom(it => it.TrustValue.ToString()))
                .ReverseMap();
        }
    }
}
