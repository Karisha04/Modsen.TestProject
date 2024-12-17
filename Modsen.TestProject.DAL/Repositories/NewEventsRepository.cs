using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL.Entities;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public class NewEventsRepository : INewEventsRepository
    {
        private readonly ProjectDbContext _context;

        public NewEventsRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<NewEvent>> Get()
        {
            var newEventEntities = await _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants) 
                .ToListAsync();

            var newEvents = newEventEntities
                .Select(MapToDomainModel) 
                .ToList();

            return newEvents;
        }

        public async Task<Guid> Create(NewEvent newEvent)
        {
            var participantEntities = newEvent.Participants.Select(p => new ParticipantEntity
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                RegistrationDate = p.RegistrationDate,
                Email = p.Email,
                NewEventId = newEvent.Id
            }).ToList();

            var newEventEntity = new NewEventEntity
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                DateAndTime = newEvent.DateAndTime,
                Place = newEvent.Place,
                Category = newEvent.Category,
                MaxParticipant = newEvent.MaxParticipant,
                Participants = participantEntities,
                ImagePath = newEvent.ImagePath
            };

            await _context.NewEvents.AddAsync(newEventEntity);
            await _context.SaveChangesAsync();

            return newEventEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath)
        {
            var participantEntities = participants.Select(p => new ParticipantEntity
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                RegistrationDate = p.RegistrationDate,
                Email = p.Email,
                NewEventId = p.NewEventId
            }).ToList();

            var newEventEntity = await _context.NewEvents
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (newEventEntity != null)
            {
                newEventEntity.Name = name;
                newEventEntity.Description = description;
                newEventEntity.DateAndTime = dateAndTime;
                newEventEntity.Place = place;
                newEventEntity.Category = category;
                newEventEntity.MaxParticipant = maxParticipant;
                newEventEntity.Participants = participantEntities;
                newEventEntity.ImagePath = imagePath;

                await _context.SaveChangesAsync();
            }

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var newEvent = await _context.NewEvents.FindAsync(id);
            if (newEvent != null)
            {
                _context.NewEvents.Remove(newEvent);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task<NewEvent> GetById(Guid id)
        {
            var newEventEntity = await _context.NewEvents
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            return newEventEntity != null ? MapToDomainModel(newEventEntity) : null;
        }

        public async Task<NewEvent> GetByName(string name)
        {
            var newEventEntity = await _context.NewEvents
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Name == name);

            return newEventEntity != null ? MapToDomainModel(newEventEntity) : null;
        }

        public async Task UpdateEvent(NewEvent newEvent)
        {
            var newEventEntity = new NewEventEntity
            {
                Id = newEvent.Id,
                Name = newEvent.Name,
                Description = newEvent.Description,
                DateAndTime = newEvent.DateAndTime,
                Place = newEvent.Place,
                Category = newEvent.Category,
                MaxParticipant = newEvent.MaxParticipant,
                ImagePath = newEvent.ImagePath,
                Participants = newEvent.Participants.Select(p => new ParticipantEntity
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    RegistrationDate = p.RegistrationDate,
                    Email = p.Email,
                    NewEventId = p.NewEventId
                }).ToList()
            };

            _context.Update(newEventEntity);
            await _context.SaveChangesAsync();
        }

        
        private NewEvent MapToDomainModel(NewEventEntity newEventEntity)
        {
            return NewEvent.Create(
                newEventEntity.Id,
                newEventEntity.Name,
                newEventEntity.Description,
                newEventEntity.DateAndTime,
                newEventEntity.Place,
                newEventEntity.Category,
                newEventEntity.MaxParticipant,
                newEventEntity.Participants.Select(p =>
                    new Participant(
                        p.Id,
                        p.FirstName,
                        p.LastName,
                        p.BirthDate,
                        p.RegistrationDate,
                        p.Email,
                        newEventEntity.Id
                    )).ToList(),
                newEventEntity.ImagePath
            ).NewEvent;
        }

        public async Task<IEnumerable<NewEvent>> GetFilteredEventsAsync(DateTime? date, string place, string category)
        {
            var query = _context.NewEvents
                .AsNoTracking()
                .Include(e => e.Participants) 
                .AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(e => e.DateAndTime.Date == date.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(place))
            {
                query = query.Where(e => e.Place.Contains(place));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category.Contains(category));
            }

            var eventEntities = await query.ToListAsync();

            return eventEntities.Select(MapToDomainModel);
        }



    }
}
