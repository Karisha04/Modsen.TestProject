using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Interfaces
{
    public interface IParticipantsRepository : IRepository<Participant>
    {
        Task<Guid> Create(Participant participant, CancellationToken cancellationToken);
        Task<Guid> Delete(Participant participant, CancellationToken cancellationToken);
        Task<List<Participant>> GetAllAsync(CancellationToken cancellationToken);
        Task<Participant> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Update(Participant participant, CancellationToken cancellationToken);
        Task<Participant> GetByEmail(string email, CancellationToken cancellationToken);
    }
}
