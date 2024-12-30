using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public class NewEventsRepository : INewEventsRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public NewEventsRepository(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NewEvent>> Get(CancellationToken cancellationToken)
        {
            var newEventEntities = await _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<NewEvent>>(newEventEntities);
        }

        public async Task<Guid> Create(NewEvent newEvent, CancellationToken cancellationToken)
        {
            var newEventEntity = _mapper.Map<NewEventEntity>(newEvent);
            await _context.NewEvents.AddAsync(newEventEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return newEventEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath, CancellationToken cancellationToken)
        {
            var newEventEntity = await _context.NewEvents.FindAsync(new object[] { id }, cancellationToken);
            if (newEventEntity != null)
            {
                newEventEntity.Name = name;
                newEventEntity.Description = description;
                newEventEntity.DateAndTime = dateAndTime;
                newEventEntity.Place = place;
                newEventEntity.Category = category;
                newEventEntity.MaxParticipant = maxParticipant;
                newEventEntity.ImagePath = imagePath;
                newEventEntity.Participants = _mapper.Map<List<ParticipantEntity>>(participants);

                _context.Update(newEventEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return id;
        }

        public async Task<Guid> Delete(Guid id, CancellationToken cancellationToken)
        {
            var newEventEntity = await _context.NewEvents.FindAsync(new object[] { id }, cancellationToken);
            if (newEventEntity != null)
            {
                _context.NewEvents.Remove(newEventEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return id;
        }

        public async Task<NewEvent> GetById(Guid id, CancellationToken cancellationToken)
        {
            var newEventEntity = await _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return _mapper.Map<NewEvent>(newEventEntity);
        }

        public async Task<NewEvent> GetByName(string name, CancellationToken cancellationToken)
        {
            var newEventEntity = await _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Name == name, cancellationToken);

            return _mapper.Map<NewEvent>(newEventEntity);
        }

        public async Task<IEnumerable<NewEvent>> GetFilteredEventsAsync(DateTime? date, string place, string category, CancellationToken cancellationToken)
        {
            var query = _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants)
                .AsQueryable();

            if (date.HasValue)
                query = query.Where(e => e.DateAndTime.Date == date.Value.Date);

            if (!string.IsNullOrWhiteSpace(place))
                query = query.Where(e => e.Place.Contains(place));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(e => e.Category.Contains(category));

            var eventEntities = await query.ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<NewEvent>>(eventEntities);
        }
        public async Task UpdateEvent(NewEvent newEvent, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var newEventEntity = _mapper.Map<NewEventEntity>(newEvent);

            _context.NewEvents.Update(newEventEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
