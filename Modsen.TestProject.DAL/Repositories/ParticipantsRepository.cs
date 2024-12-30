using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.Domain.Interfaces;
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

        public async Task<Guid> Create(Participant participant, CancellationToken cancellationToken)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(participant);
            await _context.Participants.AddAsync(participantEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return participantEntity.Id;
        }

        public async Task<Guid> Delete(Guid id, CancellationToken cancellationToken)
        {
            var participantEntity = new Entities.ParticipantEntity { Id = id };
            _context.Participants.Remove(participantEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return id;
        }

        public async Task<List<Participant>> GetAllAsync(CancellationToken cancellationToken)
        {
            var participantEntities = await _context.Participants
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Participant>>(participantEntities);
        }

        public async Task<Participant> GetById(Guid id, CancellationToken cancellationToken)
        {
            var participantEntity = await _context.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return _mapper.Map<Participant>(participantEntity);
        }

        public async Task<Participant> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var participantEntity = await _context.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);

            return _mapper.Map<Participant>(participantEntity);
        }

        public async Task<Guid> Update(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId, CancellationToken cancellationToken)
        {
            var participantEntity = new Entities.ParticipantEntity
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                RegistrationDate = registrationDate,
                Email = email,
                NewEventId = newEventId
            };

            _context.Participants.Update(participantEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return id;
        }

        // 
        public async Task<Participant> GetByIdAsync(Guid id)
        {
            var participantEntity = await _context.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<Participant>(participantEntity);
        }

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            var participantEntities = await _context.Participants
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<Participant>>(participantEntities);
        }

        public async Task AddAsync(Participant entity)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(entity);
            await _context.Participants.AddAsync(participantEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Participant entity)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(entity);
            _context.Participants.Update(participantEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var participantEntity = new Entities.ParticipantEntity { Id = id };
            _context.Participants.Remove(participantEntity);
            await _context.SaveChangesAsync();
        }
    }
}
