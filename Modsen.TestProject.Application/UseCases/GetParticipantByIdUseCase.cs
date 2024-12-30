using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.UseCases
{
    public class GetParticipantByIdUseCase
    {
        private readonly IParticipantsRepository _participantsRepository;

        public GetParticipantByIdUseCase(IParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
        }

        public async Task<Participant> Execute(Guid id, CancellationToken cancellationToken)
        {
            return await _participantsRepository.GetById(id, cancellationToken);
        }
    }
}
