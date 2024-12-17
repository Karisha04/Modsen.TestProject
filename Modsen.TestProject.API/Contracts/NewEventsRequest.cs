using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Contracts
{
    public record NewEventsRequest(
        string name,
        string description,
        DateTime dateAndTime,
        string place,
        string category,
        int maxParticipant,
        ICollection<Participant> participants,
        string imagePath);
}


