using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Interfaces
{
    public interface IParticipantsRepository : IRepository<Participant>
    {
        Task<Guid> Create(Participant participant, CancellationToken cancellationToken);
        Task<Guid> Delete(Guid id, CancellationToken cancellationToken);
        Task<List<Participant>> GetAllAsync(CancellationToken cancellationToken);
        Task<Participant> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Update(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId, CancellationToken cancellationToken);
        Task<Participant> GetByEmail(string email, CancellationToken cancellationToken);
    }
}
