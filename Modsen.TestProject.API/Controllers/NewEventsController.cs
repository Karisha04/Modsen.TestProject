using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Application.Contracts;
using Modsen.TestProject.Application.UseCases;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class NewEventsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly GetAllNewEventsUseCase _getAllNewEventsUseCase;
    private readonly CreateNewEventUseCase _createNewEventUseCase;
    private readonly UpdateNewEventUseCase _updateNewEventUseCase;
    private readonly DeleteNewEventUseCase _deleteNewEventUseCase;
    private readonly GetNewEventByIdUseCase _getNewEventByIdUseCase;
    private readonly GetNewEventByNameUseCase _getNewEventByNameUseCase;
    private readonly UpdateEventImagePathUseCase _updateEventImagePathUseCase;
    private readonly IImageService _imageService;

    public NewEventsController(
        IMapper mapper,
        GetAllNewEventsUseCase getAllNewEventsUseCase,
        CreateNewEventUseCase createNewEventUseCase,
        UpdateNewEventUseCase updateNewEventUseCase,
        DeleteNewEventUseCase deleteNewEventUseCase,
        GetNewEventByIdUseCase getNewEventByIdUseCase,
        GetNewEventByNameUseCase getNewEventByNameUseCase,
        UpdateEventImagePathUseCase updateEventImagePathUseCase,
        IImageService imageService)
    {
        _mapper = mapper;
        _getAllNewEventsUseCase = getAllNewEventsUseCase;
        _createNewEventUseCase = createNewEventUseCase;
        _updateNewEventUseCase = updateNewEventUseCase;
        _deleteNewEventUseCase = deleteNewEventUseCase;
        _getNewEventByIdUseCase = getNewEventByIdUseCase;
        _getNewEventByNameUseCase = getNewEventByNameUseCase;
        _updateEventImagePathUseCase = updateEventImagePathUseCase;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<NewEventsResponse>>> GetNewEvents(CancellationToken cancellationToken)
    {
        var newEvents = await _getAllNewEventsUseCase.Execute(cancellationToken);
        var response = newEvents.Select(b => _mapper.Map<NewEventsResponse>(b)).ToList();
        return Ok(response);
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewEvent([FromForm] NewEventsRequest request, IFormFile imageFile, CancellationToken cancellationToken)
    {
        var newEvent = _mapper.Map<NewEvent>(request);
        var newEventId = Guid.NewGuid();
        newEvent.Id = newEventId;
        var imagePath = await _imageService.UploadImageAsync(newEventId, imageFile, cancellationToken);
        newEvent.ImagePath = imagePath;
        await _createNewEventUseCase.Execute(newEvent, cancellationToken);
        return CreatedAtAction(nameof(GetNewEventById), new { id = newEventId }, newEventId);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateNewEvent(Guid id, [FromBody] NewEventsRequest request, CancellationToken cancellationToken)
    {
        await _updateNewEventUseCase.Execute(
            id, request.name, request.description, request.dateAndTime,
            request.place, request.category, request.maxParticipant,
            request.participants, request.imagePath, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNewEvent(Guid id, CancellationToken cancellationToken)
    {
        await _deleteNewEventUseCase.Execute(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NewEventsResponse>> GetNewEventById(Guid id, CancellationToken cancellationToken)
    {
        var newEvent = await _getNewEventByIdUseCase.Execute(id, cancellationToken);
        var response = _mapper.Map<NewEventsResponse>(newEvent);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("{id:guid}/upload-image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile imageFile, CancellationToken cancellationToken)
    {
        await _updateEventImagePathUseCase.Execute(id, imageFile.FileName, cancellationToken);
        return Ok(new { Message = "Image path updated successfully" });
    }


}
