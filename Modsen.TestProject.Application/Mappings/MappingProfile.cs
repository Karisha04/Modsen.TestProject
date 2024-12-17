using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.DAL.Entities;
//using Modsen.TestProject.API.Contracts;


namespace Modsen.TestProject.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewEventEntity, NewEvent>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants)); 
            CreateMap<Participant, ParticipantEntity>();
            CreateMap<ParticipantEntity, Participant>();
        }
    }
}
