using System.Text.Json;
using test.app.DataAccess;
using test.app.Models;

namespace test.app.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly HttpClient _httpClient;

        public ImageService(IImageRepository imageRepository, HttpClient httpClient)
        {
            _imageRepository = imageRepository;
            _httpClient = httpClient;
        }

        public async Task<ImageResponse> GetImageUrl(string userIdentifier)
        {          
            char lastChar = userIdentifier[^1];

            // Check if the last character is a digit
            if (char.IsDigit(lastChar) && int.TryParse(lastChar.ToString(), out int lastDigit))
            {
                if (lastDigit >= 6 && lastDigit <= 9)
                {
                    // Fetch from external API
                    var apiUrl = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastDigit}";
                    var response = await _httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var imageUrl = await JsonSerializer.DeserializeAsync<ImageResponse>(await response.Content.ReadAsStreamAsync());
                        if (imageUrl != null)
                        {
                            return new ImageResponse { Id = imageUrl.Id, Url = imageUrl.Url };
                            
                        }
                    }
                }
                else if (lastDigit >= 1 && lastDigit <= 5)
                {
                    // Fetch from database
                    var dbResponse = await _imageRepository.GetImageUrl(lastDigit);
                    if(dbResponse != null){
                        return dbResponse;
                    }                    
                }
            }

            // Check for vowels
            if (userIdentifier.Any(c => "aeiou".Contains(c, StringComparison.OrdinalIgnoreCase)))
            {
                return new ImageResponse
                {
                    Id = null,
                    Url = "https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150"
                };
            }

            // Check for non-alphanumeric characters
            if (userIdentifier.Any(c => !char.IsLetterOrDigit(c)))
            {
                int randomSeed = new Random().Next(1, 6);
                return new ImageResponse
                {
                    Id = randomSeed,
                    Url = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomSeed}&size=150"
                };
            }

            // Default fallback
            
                return new ImageResponse
                {
                    Id = null,
                    Url = "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"
                };            
        }
    }
}
