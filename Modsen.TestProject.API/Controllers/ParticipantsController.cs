using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Application.UseCases;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly GetAllParticipantsUseCase _getAllParticipantsUseCase;
        private readonly GetParticipantByIdUseCase _getParticipantByIdUseCase;
        private readonly CreateParticipantUseCase _createParticipantUseCase;
        private readonly UpdateParticipantUseCase _updateParticipantUseCase;
        private readonly DeleteParticipantUseCase _deleteParticipantUseCase;
        private readonly IValidator<ParticipantRequest> _participantValidator;

        public ParticipantsController(
            IMapper mapper,
            GetAllParticipantsUseCase getAllParticipantsUseCase,
            GetParticipantByIdUseCase getParticipantByIdUseCase,
            CreateParticipantUseCase createParticipantUseCase,
            UpdateParticipantUseCase updateParticipantUseCase,
            DeleteParticipantUseCase deleteParticipantUseCase,
            IValidator<ParticipantRequest> participantValidator)
        {
            _mapper = mapper;
            _getAllParticipantsUseCase = getAllParticipantsUseCase;
            _getParticipantByIdUseCase = getParticipantByIdUseCase;
            _createParticipantUseCase = createParticipantUseCase;
            _updateParticipantUseCase = updateParticipantUseCase;
            _deleteParticipantUseCase = deleteParticipantUseCase;
            _participantValidator = participantValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ParticipantResponse>>> GetParticipants(CancellationToken cancellationToken)
        {
            var participants = await _getAllParticipantsUseCase.Execute(cancellationToken);
            var response = participants.Select(p => _mapper.Map<ParticipantResponse>(p)).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ParticipantResponse>> GetParticipantById(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _getParticipantByIdUseCase.Execute(id, cancellationToken);
            var response = _mapper.Map<ParticipantResponse>(participant);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateParticipant([FromBody] ParticipantRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _participantValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var participant = _mapper.Map<Participant>(request);
            var participantId = await _createParticipantUseCase.Execute(participant, cancellationToken);
            return CreatedAtAction(nameof(GetParticipantById), new { id = participantId }, participantId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateParticipant(Guid id, [FromBody] ParticipantRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _participantValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _updateParticipantUseCase.Execute(
                id,
                request.FirstName,
                request.LastName,
                request.BirthDate,
                request.RegistrationDate,
                request.Email,
                request.NewEventId,
                cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteParticipant(Guid id, CancellationToken cancellationToken)
        {
            await _deleteParticipantUseCase.Execute(id, cancellationToken);
            return NoContent();
        }
    }
}
