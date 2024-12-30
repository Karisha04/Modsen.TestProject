using Microsoft.AspNetCore.Http;
using Modsen.TestProject.Domain.Interfaces;

namespace Modsen.TestProject.Application.Services
{

    public class ImageService : IImageService
    {
        public async Task<string> UploadImageAsync(Guid eventId, IFormFile imageFile, CancellationToken cancellationToken)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is required.");

            var filePath = Path.Combine("wwwroot/images/events", $"{eventId}_{imageFile.FileName}");

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream, cancellationToken);
            }

            return filePath;
        }
    }
}
