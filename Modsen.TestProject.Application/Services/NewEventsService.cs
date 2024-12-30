using FluentValidation;
using Modsen.TestProject.Application.UseCases;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.Services
{
    public class NewEventsService : INewEventsService
    {
        private readonly GetAllNewEventsUseCase _getAllNewEventsUseCase;
        private readonly CreateNewEventUseCase _createNewEventUseCase;
        private readonly UpdateNewEventUseCase _updateNewEventUseCase;
        private readonly DeleteNewEventUseCase _deleteNewEventUseCase;
        private readonly GetNewEventByIdUseCase _getNewEventByIdUseCase;
        private readonly GetNewEventByNameUseCase _getNewEventByNameUseCase;
        private readonly UpdateEventImagePathUseCase _updateEventImagePathUseCase;
        private readonly INewEventsRepository _newEventsRepository;
        private readonly IValidator<NewEvent> _newEventValidator;

        public NewEventsService(
            GetAllNewEventsUseCase getAllNewEventsUseCase,
            CreateNewEventUseCase createNewEventUseCase,
            UpdateNewEventUseCase updateNewEventUseCase,
            DeleteNewEventUseCase deleteNewEventUseCase,
            GetNewEventByIdUseCase getNewEventByIdUseCase,
            GetNewEventByNameUseCase getNewEventByNameUseCase,
            UpdateEventImagePathUseCase updateEventImagePathUseCase,
            INewEventsRepository newEventsRepository,
            IValidator<NewEvent> newEventValidator)
        {
            _getAllNewEventsUseCase = getAllNewEventsUseCase;
            _createNewEventUseCase = createNewEventUseCase;
            _updateNewEventUseCase = updateNewEventUseCase;
            _deleteNewEventUseCase = deleteNewEventUseCase;
            _getNewEventByIdUseCase = getNewEventByIdUseCase;
            _getNewEventByNameUseCase = getNewEventByNameUseCase;
            _updateEventImagePathUseCase = updateEventImagePathUseCase;
            _newEventsRepository = newEventsRepository;
            _newEventValidator = newEventValidator;
        }

        public async Task<List<NewEvent>> GetAllNewEvents(CancellationToken cancellationToken) =>
            await _getAllNewEventsUseCase.Execute(cancellationToken);

        public async Task<Guid> CreateNewEvent(NewEvent newEvent, CancellationToken cancellationToken)
        {
            var validationResult = await _newEventValidator.ValidateAsync(newEvent, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _createNewEventUseCase.Execute(newEvent, cancellationToken);
        }

        public async Task UpdateNewEvent(
            Guid id, string name, string description, DateTime dateAndTime,
            string place, string category, int maxParticipant, ICollection<Participant> participants,
            string imagePath, CancellationToken cancellationToken)
        {
            var newEvent = new NewEvent(id, name, description, dateAndTime, place, category, maxParticipant, participants, imagePath);
            var validationResult = await _newEventValidator.ValidateAsync(newEvent, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _updateNewEventUseCase.Execute(id, name, description, dateAndTime, place, category, maxParticipant, participants, imagePath, cancellationToken);
        }

        public async Task DeleteNewEvent(Guid id, CancellationToken cancellationToken) =>
            await _deleteNewEventUseCase.Execute(id, cancellationToken);

        public async Task<NewEvent> GetNewEventById(Guid id, CancellationToken cancellationToken) =>
            await _getNewEventByIdUseCase.Execute(id, cancellationToken);

        public async Task<NewEvent> GetNewEventByName(string name, CancellationToken cancellationToken) =>
            await _getNewEventByNameUseCase.Execute(name, cancellationToken);

        public async Task UpdateEventImagePath(Guid id, string imagePath, CancellationToken cancellationToken) =>
            await _updateEventImagePathUseCase.Execute(id, imagePath, cancellationToken);

        public async Task<List<NewEvent>> GetFilteredEvents(DateTime? date, string place, string category, CancellationToken cancellationToken) =>
            (await _newEventsRepository.GetFilteredEventsAsync(date, place, category, cancellationToken)).ToList();
    }
}
