using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Abstractions
{
    public interface INewEventsService
    {
        Task<Guid> CreateNewEvent(NewEvent newEvent);
        Task<Guid> DeleteNewEvent(Guid id);
        Task<List<NewEvent>> GetAllNewEvents();
        Task<Guid> UpdateNewEvent(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath);
        Task<NewEvent> GetNewEventById(Guid id);
        Task<NewEvent> GetNewEventByName(string name);
        Task UpdateEventImagePath(Guid id, string imagePath);
    }

}