using test.app.Models;

namespace test.app.DataAccess
{
    public interface IImageRepository
    {
        Task<ImageResponse?> GetImageUrl(int id);
    }
}
