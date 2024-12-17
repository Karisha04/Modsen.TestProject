using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modsen.TestProject.API.Contracts;
using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.Domain.Abstractions;
using Modsen.TestProject.Application.Mappings;

using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewEventsController : ControllerBase
    {
        private readonly INewEventsService _newEventsService;
        private readonly ILogger<NewEventsController> _logger;
        private readonly INewEventsRepository _newEventsRepository;


        public NewEventsController(INewEventsService newEventsService, ILogger<NewEventsController> logger, INewEventsRepository newEventsRepository)
        {
            _newEventsService = newEventsService;
            _logger = logger;
            _newEventsRepository = newEventsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewEventsResponse>>> GetNewEvents()
        {
            var newEvents = await _newEventsService.GetAllNewEvents();
            var response = newEvents.Select(b => new NewEventsResponse(b.Id, b.Name, b.Description, b.DateAndTime, b.Place, b.Category, b.MaxParticipant, b.Participants, b.ImagePath));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateNewEvent([FromBody] NewEventsRequest request)
        {
            var (newEvent, error) = NewEvent.Create(
                Guid.NewGuid(),
                request.name,
                request.description,
                request.dateAndTime,
                request.place,
                request.category,
                request.maxParticipant,
                request.participants,
                request.imagePath);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var newEventId = await _newEventsService.CreateNewEvent(newEvent);
            return CreatedAtAction(nameof(GetNewEventById), new { id = newEventId }, newEventId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateNewEvent(Guid id, [FromBody] NewEventsRequest request)
        {
            await _newEventsService.UpdateNewEvent(id, request.name, request.description, request.dateAndTime, request.place, request.category, request.maxParticipant, request.participants, request.imagePath);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteNewEvent(Guid id)
        {
            await _newEventsService.DeleteNewEvent(id);
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NewEventsResponse>> GetNewEventById(Guid id)
        {
            var newEvent = await _newEventsService.GetNewEventById(id);
            var response = new NewEventsResponse(newEvent.Id, newEvent.Name, newEvent.Description, newEvent.DateAndTime, newEvent.Place, newEvent.Category, newEvent.MaxParticipant, newEvent.Participants, newEvent.ImagePath);
            return Ok(response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<NewEventsResponse>> GetNewEventByName(string name)
        {
            var newEvent = await _newEventsService.GetNewEventByName(name);
            var response = new NewEventsResponse(newEvent.Id, newEvent.Name, newEvent.Description, newEvent.DateAndTime, newEvent.Place, newEvent.Category, newEvent.MaxParticipant, newEvent.Participants, newEvent.ImagePath);
            return Ok(response);
        }

        [HttpPost("{id:guid}/upload-image")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Image file is required.");

            var filePath = Path.Combine("wwwroot/images/events", $"{id}_{imageFile.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            await _newEventsService.UpdateEventImagePath(id, filePath);
            return Ok(new { ImagePath = filePath });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEvents([FromQuery] DateTime? date, [FromQuery] string place, [FromQuery] string category)
        {
            if (!date.HasValue && string.IsNullOrWhiteSpace(place) && string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("At least one filter parameter (date, place, or category) must be provided.");
            }

            var eventEntities = await _newEventsRepository.GetFilteredEventsAsync(date, place, category);

            if (!eventEntities.Any())
            {
                return NotFound("No events found for the specified criteria.");
            }

            var events = eventEntities.Select(e => new NewEventsResponse(
                e.Id,
                e.Name,
                e.Description,
                e.DateAndTime,
                e.Place,
                e.Category,
                e.MaxParticipant,
                e.Participants,
                e.ImagePath
            ));

            return Ok(events);
        }



    }
}
