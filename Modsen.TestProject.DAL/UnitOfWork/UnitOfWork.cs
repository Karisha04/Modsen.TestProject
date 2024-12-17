using Modsen.TestProject.DAL.Repositories;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDbContext _context;

        private IRepository<NewEvent> _newEventRepository;

        public UnitOfWork(ProjectDbContext context)
        {
            _context = context;
        }

        public IRepository<NewEvent> NewEventRepository
            => _newEventRepository ??= new Repository<NewEvent>(_context);

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
