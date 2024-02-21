namespace WebScraping
{
    public interface IScraperHttpClient
    {
        Task<string> GetStringAsync(Uri uri);
        Task<byte[]> GetByteArrayAsync(Uri uri);
    }

    public class ScraperHttpClient : IScraperHttpClient
    {
        private readonly HttpClient _httpClient;

        public ScraperHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            return await _httpClient.GetStringAsync(uri);
        }

        public async Task<byte[]> GetByteArrayAsync(Uri uri)
        {
            return await _httpClient.GetByteArrayAsync(uri);
        }
    }
}