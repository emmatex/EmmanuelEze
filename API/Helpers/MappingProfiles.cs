using AutoMapper;
using Domain.Entities;
using Domain.Model;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        { 
            CreateMap<PaymentRequest, Payment>().ReverseMap();
        }
    }
}
