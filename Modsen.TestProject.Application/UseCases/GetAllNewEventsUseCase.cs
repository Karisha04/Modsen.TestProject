using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.UseCases
{
    public class GetAllNewEventsUseCase
    {
        private readonly INewEventsRepository _newEventsRepository;

        public GetAllNewEventsUseCase(INewEventsRepository newEventsRepository)
        {
            _newEventsRepository = newEventsRepository;
        }

        public async Task<List<NewEvent>> Execute(CancellationToken cancellationToken)
        {
            return await _newEventsRepository.Get(cancellationToken);
        }
    }
}
