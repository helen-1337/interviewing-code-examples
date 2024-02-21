using Microsoft.Extensions.DependencyInjection;

namespace WebScraping;

internal static class Program
{
    private const string BaseUrl = "https://books.toscrape.com";
    private const string BaseDirectory = "C:/temp/DownloadedBooksToScrape";

    private static async Task Main()
    {
        Directory.CreateDirectory(BaseDirectory);

        using var scope = RegisterServices().CreateScope();
        var crawler = scope.ServiceProvider.GetRequiredService<ICrawler>();

        await crawler.DownloadCompleteSite();
    }

    private static IServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddHttpClient<IScraperHttpClient, ScraperHttpClient>();
        services.AddTransient<ICrawler, Crawler>();
        services.AddSingleton<IConfiguration>(new Configuration(BaseUrl, BaseDirectory));

        return services.BuildServiceProvider();
    }
}