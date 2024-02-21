namespace WebScraping;

public interface IConfiguration
{
    string BaseUrl { get; }
    string BaseDirectory { get; }
}

public class Configuration : IConfiguration
{
    public string BaseUrl { get; }
    public string BaseDirectory { get; }


    public Configuration(string baseUrl, string baseDirectory)
    {
        BaseUrl = baseUrl;
        BaseDirectory = baseDirectory;
    }
}
