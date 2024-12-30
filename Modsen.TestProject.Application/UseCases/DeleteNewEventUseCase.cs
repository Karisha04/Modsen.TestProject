using Modsen.TestProject.Domain.Interfaces;

public class DeleteNewEventUseCase
{
    private readonly INewEventsRepository _newEventsRepository;

    public DeleteNewEventUseCase(INewEventsRepository newEventsRepository)
    {
        _newEventsRepository = newEventsRepository;
    }

    public async Task<Guid> Execute(Guid id, CancellationToken cancellationToken)
    {
        var existingEvent = await _newEventsRepository.GetById(id, cancellationToken);
        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Event with ID '{id}' not found.");
        }

        return await _newEventsRepository.Delete(id, cancellationToken);
    }
}
