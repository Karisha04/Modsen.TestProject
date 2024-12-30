using Modsen.TestProject.Domain.Interfaces;

public class UpdateParticipantUseCase
{
    private readonly IParticipantsRepository _participantsRepository;

    public UpdateParticipantUseCase(IParticipantsRepository participantsRepository)
    {
        _participantsRepository = participantsRepository;
    }

    public async Task<Guid> Execute(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId, CancellationToken cancellationToken)
    {
        var existingParticipant = await _participantsRepository.GetById(id, cancellationToken);
        if (existingParticipant == null)
        {
            throw new KeyNotFoundException($"Participant with ID '{id}' not found.");
        }

        var participantWithSameEmail = await _participantsRepository.GetByEmail(email, cancellationToken);
        if (participantWithSameEmail != null && participantWithSameEmail.Id != id)
        {
            throw new InvalidOperationException($"A participant with the email '{email}' already exists.");
        }

        return await _participantsRepository.Update(id, firstName, lastName, birthDate, registrationDate, email, newEventId, cancellationToken);
    }
}
