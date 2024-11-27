using Microsoft.Data.Sqlite;
using test.app.Models;

namespace test.app.DataAccess
{
    public class ImageRepository : IImageRepository
    {
        private const string ConnectionString = "Data Source=data.db";

        public async Task<ImageResponse?> GetImageUrl(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT id, url FROM images WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ImageResponse
                {
                    Id = reader.GetInt32(0),
                    Url = reader.GetString(1)
                };
            }

            return null;
        }
    }
}
