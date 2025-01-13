using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Application.Contracts;

namespace Modsen.TestProject.Application.Mappings
{
    public class NewEventEntityMappingProfile : Profile
    {
        public NewEventEntityMappingProfile()
        {
            CreateMap<NewEventsRequest, NewEvent>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.participants));
        }
    }
}
