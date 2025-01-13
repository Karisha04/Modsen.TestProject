using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public class NewEventsRepository : Repository<NewEventEntity>, INewEventsRepository
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public NewEventsRepository(ProjectDbContext context, IMapper mapper) : base(context)
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
            return newEventEntity.Id;
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
        public async Task<Guid> Update(NewEvent newEvent, CancellationToken cancellationToken)
        {
            var newEventEntity = _mapper.Map<Entities.NewEventEntity>(newEvent);
            _context.NewEvents.Update(newEventEntity);
            return newEventEntity.Id;
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

        public async Task<Guid> Delete(Guid id, CancellationToken cancellationToken)
        {
            var newEventEntity = new NewEventEntity { Id = id };  
            _context.NewEvents.Remove(newEventEntity);  
            return newEventEntity.Id;  
        }

        public async Task UpdateEvent(NewEvent newEvent, CancellationToken cancellationToken)
        {
            var newEventEntity = _mapper.Map<NewEventEntity>(newEvent);  
            _context.NewEvents.Update(newEventEntity);  
        }
    }
}
