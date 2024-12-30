using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

public class CreateNewEventUseCase
{
    private readonly INewEventsRepository _newEventsRepository;

    public CreateNewEventUseCase(INewEventsRepository newEventsRepository)
    {
        _newEventsRepository = newEventsRepository;
    }

    public async Task<Guid> Execute(NewEvent newEvent, CancellationToken cancellationToken)
    {
        var existingEvent = await _newEventsRepository.GetByName(newEvent.Name, cancellationToken);
        if (existingEvent != null)
        {
            throw new InvalidOperationException($"An event with the name '{newEvent.Name}' already exists.");
        }

        return await _newEventsRepository.Create(newEvent, cancellationToken);
    }
}
