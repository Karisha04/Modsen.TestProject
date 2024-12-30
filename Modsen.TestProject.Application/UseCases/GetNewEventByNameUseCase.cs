using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.UseCases
{
    public class GetNewEventByNameUseCase
    {
        private readonly INewEventsRepository _newEventsRepository;

        public GetNewEventByNameUseCase(INewEventsRepository newEventsRepository)
        {
            _newEventsRepository = newEventsRepository;
        }

        public async Task<NewEvent> Execute(string name, CancellationToken cancellationToken)
        {
            return await _newEventsRepository.GetByName(name, cancellationToken);
        }
    }
}
