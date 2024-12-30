using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantsService _participantsService;
        private readonly IMapper _mapper;

        public ParticipantsController(IParticipantsService participantsService, IMapper mapper)
        {
            _participantsService = participantsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ParticipantResponse>>> GetParticipants(CancellationToken cancellationToken)
        {
            var participants = await _participantsService.GetAllParticipants(cancellationToken);
            var response = participants.Select(p => new ParticipantResponse(
                p.Id, p.FirstName, p.LastName, p.BirthDate,
                p.RegistrationDate, p.Email, p.NewEventId)).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ParticipantResponse>> GetParticipantById(Guid id, CancellationToken cancellationToken)
        {
            var participant = await _participantsService.GetParticipantById(id, cancellationToken);
            if (participant == null)
            {
                return NotFound();
            }

            var response = new ParticipantResponse(
                participant.Id, participant.FirstName, participant.LastName,
                participant.BirthDate, participant.RegistrationDate,
                participant.Email, participant.NewEventId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateParticipant([FromBody] ParticipantRequest request, CancellationToken cancellationToken)
        {
            var participant = _mapper.Map<Participant>(request);
            var participantId = await _participantsService.CreateParticipant(participant, cancellationToken);
            return Ok(participantId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateParticipant(Guid id, [FromBody] ParticipantRequest request, CancellationToken cancellationToken)
        {
            var participant = _mapper.Map<Participant>(request);
            var updatedParticipantId = await _participantsService.UpdateParticipant(
                id,
                participant.FirstName,
                participant.LastName,
                participant.BirthDate,
                participant.RegistrationDate,
                participant.Email,
                participant.NewEventId,
                cancellationToken);

            return Ok(updatedParticipantId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteParticipant(Guid id, CancellationToken cancellationToken)
        {
            await _participantsService.DeleteParticipant(id, cancellationToken);
            return Ok(id);
        }
    }
}
