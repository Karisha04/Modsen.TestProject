using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

public class UpdateNewEventUseCase
{
    private readonly INewEventsRepository _newEventsRepository;

    public UpdateNewEventUseCase(INewEventsRepository newEventsRepository)
    {
        _newEventsRepository = newEventsRepository;
    }

    public async Task<Guid> Execute(
        Guid id, string name, string description, DateTime dateAndTime, string place, string category,
        int maxParticipant, ICollection<Participant> participants, string imagePath,
        CancellationToken cancellationToken)
    {
        var existingEvent = await _newEventsRepository.GetById(id, cancellationToken);
        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Event with ID '{id}' not found.");
        }

        var eventWithSameName = await _newEventsRepository.GetByName(name, cancellationToken);
        if (eventWithSameName != null && eventWithSameName.Id != id)
        {
            throw new InvalidOperationException($"An event with the name '{name}' already exists.");
        }

        return await _newEventsRepository.Update(id, name, description, dateAndTime, place, category,
                                                  maxParticipant, participants, imagePath, cancellationToken);
    }
}
