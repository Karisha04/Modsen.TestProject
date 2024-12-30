using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.DAL.Entities;

namespace Modsen.TestProject.Application.Mappings
{
    public class NewEventMappingProfile : Profile
    {
        public NewEventMappingProfile()
        {
            CreateMap<NewEventEntity, NewEvent>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));
            CreateMap<NewEvent, NewEventEntity>();
        }
    }
}
