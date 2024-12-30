using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.UseCases
{
    public class GetAllParticipantsUseCase
    {
        private readonly IParticipantsRepository _participantsRepository;

        public GetAllParticipantsUseCase(IParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
        }

        public async Task<List<Participant>> Execute(CancellationToken cancellationToken)
        {
            return await _participantsRepository.GetAllAsync(cancellationToken);
        }
    }
}
