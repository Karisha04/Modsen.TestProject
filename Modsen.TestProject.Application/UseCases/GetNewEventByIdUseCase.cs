using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.UseCases
{
    public class GetNewEventByIdUseCase
    {
        private readonly INewEventsRepository _newEventsRepository;

        public GetNewEventByIdUseCase(INewEventsRepository newEventsRepository)
        {
            _newEventsRepository = newEventsRepository;
        }

        public async Task<NewEvent> Execute(Guid id, CancellationToken cancellationToken)
        {
            return await _newEventsRepository.GetById(id, cancellationToken);
        }
    }
}
