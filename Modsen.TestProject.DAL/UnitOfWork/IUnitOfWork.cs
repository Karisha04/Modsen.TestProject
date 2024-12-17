using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<NewEvent> NewEventRepository { get; }
        Task<int> CompleteAsync(); 
    }
}
