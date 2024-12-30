using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Interfaces
{
    public interface INewEventsRepository
    {
        Task<List<NewEvent>> Get(CancellationToken cancellationToken);
        Task<Guid> Create(NewEvent newEvent, CancellationToken cancellationToken);
        Task<Guid> Update(Guid id, string name, string description, DateTime dateAndTime, string place, string category, int maxParticipant, ICollection<Participant> participants, string imagePath, CancellationToken cancellationToken);
        Task<Guid> Delete(Guid id, CancellationToken cancellationToken);
        Task<NewEvent> GetById(Guid id, CancellationToken cancellationToken);
        Task<NewEvent> GetByName(string name, CancellationToken cancellationToken);
        Task<IEnumerable<NewEvent>> GetFilteredEventsAsync(DateTime? date, string place, string category, CancellationToken cancellationToken);
        Task UpdateEvent(NewEvent newEvent, CancellationToken cancellationToken);

    }
}
