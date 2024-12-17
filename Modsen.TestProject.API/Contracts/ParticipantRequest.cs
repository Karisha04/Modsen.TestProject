using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Contracts
{
    public record ParticipantRequest(
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime RegistrationDate,
        string Email,
        Guid NewEventId);

}
