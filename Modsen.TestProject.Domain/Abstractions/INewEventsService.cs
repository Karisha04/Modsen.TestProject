using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Abstractions
{
    public interface INewEventsService
    {
        Task<List<NewEvent>> GetAllNewEvents(CancellationToken cancellationToken);
        Task<Guid> CreateNewEvent(NewEvent newEvent, CancellationToken cancellationToken);
        Task UpdateNewEvent(
            Guid id, string name, string description, DateTime dateAndTime,
            string place, string category, int maxParticipant, ICollection<Participant> participants,
            string imagePath, CancellationToken cancellationToken);
        Task DeleteNewEvent(Guid id, CancellationToken cancellationToken);
        Task<NewEvent> GetNewEventById(Guid id, CancellationToken cancellationToken);
        Task<NewEvent> GetNewEventByName(string name, CancellationToken cancellationToken);
        Task UpdateEventImagePath(Guid id, string imagePath, CancellationToken cancellationToken);
        Task<List<NewEvent>> GetFilteredEvents(DateTime? date, string place, string category, CancellationToken cancellationToken);
    }


}