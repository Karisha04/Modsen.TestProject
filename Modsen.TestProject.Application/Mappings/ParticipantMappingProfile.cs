using AutoMapper;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.DAL.Entities;

namespace Modsen.TestProject.Application.Mappings
{
    public class ParticipantMappingProfile : Profile
    {
        public ParticipantMappingProfile()
        {
            CreateMap<ParticipantEntity, Participant>();
            CreateMap<Participant, ParticipantEntity>();
        }
    }
}
