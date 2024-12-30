using AutoMapper;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Domain.Models;

public class PRequestMappingProfile : Profile
{
    public PRequestMappingProfile()
    {
        CreateMap<ParticipantRequest, Participant>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.NewEventId, opt => opt.MapFrom(src => src.NewEventId));
    }
}

