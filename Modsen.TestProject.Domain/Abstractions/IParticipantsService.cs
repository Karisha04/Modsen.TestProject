using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Abstractions
{
    public interface IParticipantsService
    {
        
            Task<Guid> CreateParticipant(Participant participant);
            Task<Guid> DeleteParticipant(Guid id);
            Task<List<Participant>> GetAllParticipants();
            Task<Participant> GetParticipantById(Guid id);
            Task<Guid> UpdateParticipant(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId);
        
    }
}