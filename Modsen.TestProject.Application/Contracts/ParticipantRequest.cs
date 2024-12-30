namespace Modsen.TestProject.Application.Contracts
{
    public record ParticipantRequest(
        string FirstName,
        string LastName,
        DateTime BirthDate,
        DateTime RegistrationDate,
        string Email,
        Guid NewEventId);

}
