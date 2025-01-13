using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Application.Contracts;

public class NERequestMappingProfile : Profile
{
    public NERequestMappingProfile()
    {
        CreateMap<NewEventsRequest, NewEvent>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))  
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.participants));  
    }
}




    