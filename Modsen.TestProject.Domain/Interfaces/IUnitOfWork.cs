using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<NewEvent> NewEventRepository { get; }
        IRepository<Participant> ParticipantRepository { get; }
        Task<int> CompleteAsync();
    }
}
