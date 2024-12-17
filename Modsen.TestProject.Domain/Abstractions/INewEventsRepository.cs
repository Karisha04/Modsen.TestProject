using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.Repositories
{
    public interface INewEventsRepository
    {
        Task<Guid> Create(NewEvent newEvent);
        Task<Guid> Delete(Guid id);
        Task<List<NewEvent>> Get();
        Task<Guid> Update(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath);
        Task<NewEvent> GetById(Guid id);
        Task<NewEvent> GetByName(string name);
        Task UpdateEvent(NewEvent newEvent);
        Task<IEnumerable<NewEvent>> GetFilteredEventsAsync(DateTime? date, string place, string category);

    }

}