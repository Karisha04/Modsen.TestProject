using Modsen.TestProject.Domain.Interfaces;

namespace Modsen.TestProject.Application.UseCases
{
    public class DeleteParticipantUseCase
    {
        private readonly IParticipantsRepository _participantsRepository;

        public DeleteParticipantUseCase(IParticipantsRepository participantsRepository)
        {
            _participantsRepository = participantsRepository;
        }

        public async Task<Guid> Execute(Guid id, CancellationToken cancellationToken)
        {
            var existingParticipant = await _participantsRepository.GetById(id, cancellationToken);
            if (existingParticipant == null)
            {
                throw new KeyNotFoundException($"Participant with ID '{id}' not found.");
            }

            return await _participantsRepository.Delete(id, cancellationToken);
        }
    }
}
