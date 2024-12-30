using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewEventsController : ControllerBase
    {
        private readonly INewEventsService _newEventsService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public NewEventsController(INewEventsService newEventsService, IImageService imageService, IMapper mapper)
        {
            _newEventsService = newEventsService;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewEventsResponse>>> GetNewEvents(CancellationToken cancellationToken)
        {
            var newEvents = await _newEventsService.GetAllNewEvents(cancellationToken);
            var response = newEvents.Select(b => new NewEventsResponse(
                b.Id, b.Name, b.Description, b.DateAndTime, b.Place,
                b.Category, b.MaxParticipant, b.Participants, b.ImagePath));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateNewEvent([FromBody] NewEventsRequest request, CancellationToken cancellationToken)
        {
            var newEvent = _mapper.Map<NewEvent>(request);
            var (createdNewEvent, error) = NewEvent.Create(
                newEvent.Id, newEvent.Name, newEvent.Description,
                newEvent.DateAndTime, newEvent.Place, newEvent.Category,
                newEvent.MaxParticipant, newEvent.Participants, newEvent.ImagePath);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var newEventId = await _newEventsService.CreateNewEvent(createdNewEvent, cancellationToken);
            return CreatedAtAction(nameof(GetNewEventById), new { id = newEventId }, newEventId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateNewEvent(Guid id, [FromBody] NewEventsRequest request, CancellationToken cancellationToken)
        {
            await _newEventsService.UpdateNewEvent(
                id, request.name, request.description, request.dateAndTime,
                request.place, request.category, request.maxParticipant,
                request.participants, request.imagePath, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteNewEvent(Guid id, CancellationToken cancellationToken)
        {
            await _newEventsService.DeleteNewEvent(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NewEventsResponse>> GetNewEventById(Guid id, CancellationToken cancellationToken)
        {
            var newEvent = await _newEventsService.GetNewEventById(id, cancellationToken);
            var response = new NewEventsResponse(
                newEvent.Id, newEvent.Name, newEvent.Description,
                newEvent.DateAndTime, newEvent.Place, newEvent.Category,
                newEvent.MaxParticipant, newEvent.Participants, newEvent.ImagePath);
            return Ok(response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<NewEventsResponse>> GetNewEventByName(string name, CancellationToken cancellationToken)
        {
            var newEvent = await _newEventsService.GetNewEventByName(name, cancellationToken);
            var response = new NewEventsResponse(
                newEvent.Id, newEvent.Name, newEvent.Description,
                newEvent.DateAndTime, newEvent.Place, newEvent.Category,
                newEvent.MaxParticipant, newEvent.Participants, newEvent.ImagePath);
            return Ok(response);
        }

        [HttpPost("{id:guid}/upload-image")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile imageFile, CancellationToken cancellationToken)
        {
            try
            {
                var filePath = await _imageService.UploadImageAsync(id, imageFile, cancellationToken);
                await _newEventsService.UpdateEventImagePath(id, filePath, cancellationToken);
                return Ok(new { ImagePath = filePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEvents([FromQuery] DateTime? date, [FromQuery] string place, [FromQuery] string category, CancellationToken cancellationToken)
        {
            if (!date.HasValue && string.IsNullOrWhiteSpace(place) && string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("At least one filter parameter (date, place, or category) must be provided.");
            }

            var eventEntities = await _newEventsService.GetFilteredEvents(date, place, category, cancellationToken);

            if (!eventEntities.Any())
            {
                return NotFound("No events found for the specified criteria.");
            }

            var events = eventEntities.Select(e => new NewEventsResponse(
                e.Id, e.Name, e.Description, e.DateAndTime, e.Place,
                e.Category, e.MaxParticipant, e.Participants, e.ImagePath));

            return Ok(events);
        }
    }
}
