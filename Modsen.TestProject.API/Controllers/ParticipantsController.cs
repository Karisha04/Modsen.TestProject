using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.API.Contracts;
using Modsen.TestProject.Application.Mappings;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Application.Services;


namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantsService _participantsService;

        public ParticipantsController(IParticipantsService participantsService)
        {
            _participantsService = participantsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ParticipantResponse>>> GetParticipants()
        {
            var participants = await _participantsService.GetAllParticipants();
            var response = participants.Select(p => new ParticipantResponse(
                p.Id, p.FirstName, p.LastName, p.BirthDate,
                p.RegistrationDate, p.Email, p.NewEventId)).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ParticipantResponse>> GetParticipantById(Guid id)
        {
            var participant = await _participantsService.GetParticipantById(id);
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
        public async Task<ActionResult<Guid>> CreateParticipant([FromBody] ParticipantRequest request)
        {
            var (participant, error) = Participant.Create(
                Guid.NewGuid(),
                request.FirstName,
                request.LastName,
                request.BirthDate,
                request.RegistrationDate,
                request.Email,
                request.NewEventId);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var participantId = await _participantsService.CreateParticipant(participant);
            return Ok(participantId);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateParticipant(Guid id, [FromBody] ParticipantRequest request)
        {
            var updatedParticipantId = await _participantsService.UpdateParticipant(
                id, request.FirstName, request.LastName, request.BirthDate,
                request.RegistrationDate, request.Email, request.NewEventId);
            return Ok(updatedParticipantId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteParticipant(Guid id)
        {
            await _participantsService.DeleteParticipant(id);
            return Ok(id);
        }
    }
}
