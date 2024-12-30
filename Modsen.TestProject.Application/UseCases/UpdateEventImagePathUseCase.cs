using Modsen.TestProject.Domain.Interfaces;

public class UpdateEventImagePathUseCase
{
    private readonly INewEventsRepository _newEventsRepository;

    public UpdateEventImagePathUseCase(INewEventsRepository newEventsRepository)
    {
        _newEventsRepository = newEventsRepository;
    }

    public async Task Execute(Guid id, string imagePath, CancellationToken cancellationToken)
    {
        var newEvent = await _newEventsRepository.GetById(id, cancellationToken);
        if (newEvent == null)
            throw new KeyNotFoundException($"Event with id {id} not found.");

        newEvent.UpdateImagePath(imagePath);
        await _newEventsRepository.UpdateEvent(newEvent, cancellationToken);
    }
}
