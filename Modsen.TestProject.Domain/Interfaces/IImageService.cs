using Microsoft.AspNetCore.Http;

namespace Modsen.TestProject.Domain.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(Guid eventId, IFormFile imageFile, CancellationToken cancellationToken);
    }
}
