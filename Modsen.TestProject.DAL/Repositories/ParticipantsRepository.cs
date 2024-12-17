using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public class ParticipantsRepository : IParticipantsRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public ParticipantsRepository(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Create(Domain.Models.Participant participant)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(participant);

            await _context.Participants.AddAsync(participantEntity);
            await _context.SaveChangesAsync();

            return participantEntity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var participantEntity = await _context.Participants
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (participantEntity == null)
            {
                return Guid.Empty;
            }

            _context.Participants.Remove(participantEntity);
            await _context.SaveChangesAsync();

            return participantEntity.Id;
        }

        public async Task<List<Domain.Models.Participant>> Get()
        {
            var participantEntities = await _context.Participants
                .AsNoTracking()
                .ToListAsync();

            var participants = _mapper.Map<List<Domain.Models.Participant>>(participantEntities);
            return participants;
        }

        public async Task<Domain.Models.Participant> GetById(Guid id)
        {
            var participantEntity = await _context.Participants
                .FirstOrDefaultAsync(p => p.Id == id);

            if (participantEntity == null)
            {
                return null;
            }

            var participant = _mapper.Map<Domain.Models.Participant>(participantEntity);
            return participant;
        }

        public async Task<Guid> Update(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId)
        {
            var participantEntity = await _context.Participants.FindAsync(id);
            if (participantEntity == null)
                throw new KeyNotFoundException("Participant not found");

            participantEntity.FirstName = firstName;
            participantEntity.LastName = lastName;
            participantEntity.BirthDate = birthDate;
            participantEntity.RegistrationDate = registrationDate;
            participantEntity.Email = email;

            _context.Participants.Update(participantEntity);
            await _context.SaveChangesAsync();

            return participantEntity.Id;
        }
    }
}
