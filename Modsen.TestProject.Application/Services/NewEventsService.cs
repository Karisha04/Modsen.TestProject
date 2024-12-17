using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.Services
{
    public class NewEventsService : INewEventsService
    {
        private readonly INewEventsRepository _newEventsRepository;

        public NewEventsService(INewEventsRepository newEventsRepository)
        {
            _newEventsRepository = newEventsRepository;
        }

        public async Task<List<NewEvent>> GetAllNewEvents() => await _newEventsRepository.Get();

        public async Task<Guid> CreateNewEvent(NewEvent newEvent) => await _newEventsRepository.Create(newEvent);

        public async Task<Guid> UpdateNewEvent(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath)
            => await _newEventsRepository.Update(id, name, description, dateAndTime, place, category, maxParticipant, participants, imagePath);

        public async Task<Guid> DeleteNewEvent(Guid id) => await _newEventsRepository.Delete(id);

        public async Task<NewEvent> GetNewEventById(Guid id) => await _newEventsRepository.GetById(id);

        public async Task<NewEvent> GetNewEventByName(string name) => await _newEventsRepository.GetByName(name);

        public async Task UpdateEventImagePath(Guid id, string imagePath)
        {
            var newEvent = await _newEventsRepository.GetById(id);
            if (newEvent == null)
                throw new KeyNotFoundException($"Event with id {id} not found.");

            newEvent.UpdateImagePath(imagePath);
            await _newEventsRepository.UpdateEvent(newEvent);
        }

    }
}
