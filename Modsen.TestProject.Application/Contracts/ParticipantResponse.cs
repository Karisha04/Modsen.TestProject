namespace Modsen.TestProject.Application.Contracts
{
    public record ParticipantResponse(
        Guid Id,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime RegistrationDate,
        string Email,
        Guid NewEventId);
}
