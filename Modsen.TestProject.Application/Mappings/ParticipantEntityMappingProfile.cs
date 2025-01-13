using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Application.Contracts;

namespace Modsen.TestProject.Application.Mappings
{
    public class ParticipantEntityMappingProfile : Profile
    {
        public ParticipantEntityMappingProfile()
        {
            CreateMap<ParticipantRequest, Participant>();
        }
    }
}
