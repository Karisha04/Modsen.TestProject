using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Abstractions
{
    public interface IParticipantsRepository
    {
        
        Task<Guid> Create(Participant participant);
        Task<Guid> Delete(Guid id);
        Task<List<Participant>> Get();
        Task<Participant> GetById(Guid id);
        Task<Guid> Update(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId);
    }
}
