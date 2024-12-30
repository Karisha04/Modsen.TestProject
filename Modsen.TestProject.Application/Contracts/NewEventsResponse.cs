using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.Contracts
{
    public record NewEventsResponse(
        Guid id,
        string name,
        string description,
        DateTime dateAndTime, 
        string place, 
        string category, 
        int maxParticipant, 
        ICollection<Participant> participants, 
        string imagePath);
}


