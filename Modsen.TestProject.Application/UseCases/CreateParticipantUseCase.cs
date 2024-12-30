using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

public class CreateParticipantUseCase
{
    private readonly IParticipantsRepository _participantsRepository;

    public CreateParticipantUseCase(IParticipantsRepository participantsRepository)
    {
        _participantsRepository = participantsRepository;
    }

    public async Task<Guid> Execute(Participant participant, CancellationToken cancellationToken)
    {
        var existingParticipant = await _participantsRepository.GetByEmail(participant.Email, cancellationToken);
        if (existingParticipant != null)
        {
            throw new InvalidOperationException($"A participant with the email '{participant.Email}' already exists.");
        }

        return await _participantsRepository.Create(participant, cancellationToken);
    }
}
