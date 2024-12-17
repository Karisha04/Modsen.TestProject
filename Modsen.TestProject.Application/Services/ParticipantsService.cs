using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Models;
using AutoMapper;
using Modsen.TestProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Repositories;


namespace Modsen.TestProject.Application.Services
{
    public class ParticipantsService : IParticipantsService
    {
        private readonly IParticipantsRepository _participantsRepository;
        private readonly IMapper _mapper;

        public ParticipantsService(IParticipantsRepository participantsRepository, IMapper mapper)
        {
            _participantsRepository = participantsRepository;
            _mapper = mapper;
        }

        public async Task<List<Participant>> GetAllParticipants()
        {
            return await _participantsRepository.Get();
        }
        public async Task<Participant> GetParticipantById(Guid id)
        {
            return await _participantsRepository.GetById(id);
        }

        public async Task<Guid> UpdateParticipant(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId)
        {
            return await _participantsRepository.Update(id, firstName, lastName, birthDate, registrationDate, email, newEventId);
        }

        public async Task<Guid> DeleteParticipant(Guid id)
        {
            return await _participantsRepository.Delete(id);
        }

        public async Task<Guid> CreateParticipant(Participant participant)
        {
            var participantEntity = _mapper.Map<Participant>(participant);

            return await _participantsRepository.Create(participantEntity);
        }
    }
}
