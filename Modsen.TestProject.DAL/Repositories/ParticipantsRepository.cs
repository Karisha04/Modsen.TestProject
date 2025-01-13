using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public class ParticipantsRepository : Repository<ParticipantEntity>, IParticipantsRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public ParticipantsRepository(ProjectDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Guid> Create(Participant participant, CancellationToken cancellationToken)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(participant);
            await _context.Participants.AddAsync(participantEntity, cancellationToken);
            return participantEntity.Id;
        }

        public async Task<Guid> Delete(Participant participant, CancellationToken cancellationToken)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(participant);
            _context.Participants.Remove(participantEntity);
            return participantEntity.Id;
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

        public async Task<Guid> Update(Participant participant, CancellationToken cancellationToken)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(participant);
            _context.Participants.Update(participantEntity);
            return participantEntity.Id;
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
        }

        public async Task UpdateAsync(Participant entity)
        {
            var participantEntity = _mapper.Map<Entities.ParticipantEntity>(entity);
            _context.Participants.Update(participantEntity);
        }


        public async Task DeleteAsync(Guid id)
        {
            var participantEntity = new Entities.ParticipantEntity { Id = id };
            _context.Participants.Remove(participantEntity);
        }
        public async Task<Participant> GetByIdAsync(Guid id)
        {
            var participantEntity = await _context.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<Participant>(participantEntity);
        }
    }
}
