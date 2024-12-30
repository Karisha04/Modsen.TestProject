using FluentValidation;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Application.UseCases;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.Services
{
    public class ParticipantsService : IParticipantsService
    {
        private readonly GetAllParticipantsUseCase _getAllParticipantsUseCase;
        private readonly GetParticipantByIdUseCase _getParticipantByIdUseCase;
        private readonly UpdateParticipantUseCase _updateParticipantUseCase;
        private readonly DeleteParticipantUseCase _deleteParticipantUseCase;
        private readonly CreateParticipantUseCase _createParticipantUseCase;
        private readonly IValidator<ParticipantRequest> _participantValidator;

        public ParticipantsService(
            GetAllParticipantsUseCase getAllParticipantsUseCase,
            GetParticipantByIdUseCase getParticipantByIdUseCase,
            UpdateParticipantUseCase updateParticipantUseCase,
            DeleteParticipantUseCase deleteParticipantUseCase,
            CreateParticipantUseCase createParticipantUseCase,
            IValidator<ParticipantRequest> participantValidator)
        {
            _getAllParticipantsUseCase = getAllParticipantsUseCase;
            _getParticipantByIdUseCase = getParticipantByIdUseCase;
            _updateParticipantUseCase = updateParticipantUseCase;
            _deleteParticipantUseCase = deleteParticipantUseCase;
            _createParticipantUseCase = createParticipantUseCase;
            _participantValidator = participantValidator;
        }

        public async Task<List<Participant>> GetAllParticipants(CancellationToken cancellationToken)
            => await _getAllParticipantsUseCase.Execute(cancellationToken);

        public async Task<Participant> GetParticipantById(Guid id, CancellationToken cancellationToken)
            => await _getParticipantByIdUseCase.Execute(id, cancellationToken);

        public async Task<Guid> UpdateParticipant(Guid id, string firstName, string lastName, DateTime birthDate, DateTime registrationDate, string email, Guid newEventId, CancellationToken cancellationToken)
        {
            var participantRequest = new ParticipantRequest(firstName, lastName, birthDate, registrationDate, email, newEventId);
            var validationResult = await _participantValidator.ValidateAsync(participantRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _updateParticipantUseCase.Execute(id, firstName, lastName, birthDate, registrationDate, email, newEventId, cancellationToken);
        }


        public async Task<Guid> DeleteParticipant(Guid id, CancellationToken cancellationToken)
            => await _deleteParticipantUseCase.Execute(id, cancellationToken);

        public async Task<Guid> CreateParticipant(Participant participant, CancellationToken cancellationToken)
        {
            var participantRequest = new ParticipantRequest(participant.FirstName, participant.LastName, participant.BirthDate, participant.RegistrationDate, participant.Email, participant.NewEventId); // Передаем все необходимые параметры
            var validationResult = await _participantValidator.ValidateAsync(participantRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            return await _createParticipantUseCase.Execute(participant, cancellationToken);
        }

    }
}
