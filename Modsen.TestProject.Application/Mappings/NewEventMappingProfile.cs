using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Application.Contracts;

namespace Modsen.TestProject.Application.Mappings
{
    public class NewEventMappingProfile : Profile
    {
        public NewEventMappingProfile()
        {
            
            CreateMap<NewEvent, NewEventsResponse>();
        }
    }
}
