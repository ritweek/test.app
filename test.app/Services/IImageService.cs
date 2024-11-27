using test.app.Models;

namespace test.app.Services
{
    public interface IImageService
    {
        Task<ImageResponse> GetImageUrl(string userIdentifier);
    }
}
