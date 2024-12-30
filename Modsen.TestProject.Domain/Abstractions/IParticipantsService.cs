using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Abstractions
{
    public interface IParticipantsService
    {
        Task<Guid> CreateParticipant(Participant participant, CancellationToken cancellationToken);
        Task<Guid> DeleteParticipant(Guid id, CancellationToken cancellationToken);
        Task<List<Participant>> GetAllParticipants(CancellationToken cancellationToken);
        Task<Participant> GetParticipantById(Guid id, CancellationToken cancellationToken);
        Task<Guid> UpdateParticipant(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId, CancellationToken cancellationToken);
    }
}
