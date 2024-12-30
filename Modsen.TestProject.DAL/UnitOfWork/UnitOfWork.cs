using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;
using Modsen.TestProject.Domain.UnitOfWork;

namespace Modsen.TestProject.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext _context;
        public IRepository<NewEvent> NewEventRepository { get; }
        public IRepository<Participant> ParticipantRepository { get; }

        public UnitOfWork(ProjectDbContext context,
                          IRepository<NewEvent> newEventRepository,
                          IRepository<Participant> participantRepository)
        {
            _context = context;
            NewEventRepository = newEventRepository;
            ParticipantRepository = participantRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
