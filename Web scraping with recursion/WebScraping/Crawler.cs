using HtmlAgilityPack;

namespace WebScraping;

public interface ICrawler
{
    Task DownloadCompleteSite();
}

public class Crawler : ICrawler
{
    private readonly HashSet<string> _resourcesToDownload = new();
    private readonly HashSet<string> _downloadedHtmlPages = new();

    private readonly IScraperHttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public Crawler(IScraperHttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task DownloadCompleteSite()
    {
        await DownloadPage(new Uri(_configuration.BaseUrl));
        await DownloadResources();
    }

    private async Task DownloadPage(Uri uri)
    {
        if (_downloadedHtmlPages.Contains(uri.ToString()))
        {
            return;
        }

        Console.WriteLine($"Downloading {uri}");

        var currentPath = UriHelper.GetRelativePathWithoutFileName(uri);

        var content = await _httpClient.GetStringAsync(uri);
        _downloadedHtmlPages.Add(uri.ToString());
        await SaveToFile(content, uri);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);
        AddResourcesToDownload(htmlDoc, currentPath, _configuration.BaseUrl);

        var links = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
        foreach (var link in links)
        {
            var linkRelativeUrl = link.Attributes["href"].Value;
            if (!Uri.IsWellFormedUriString(linkRelativeUrl, UriKind.Relative))
            {
                continue;
            }

            var absoluteUrl = UriHelper.CreateAbsoluteUri(_configuration.BaseUrl, currentPath, linkRelativeUrl);
            await DownloadPage(absoluteUrl);
        }
    }

    private void AddResourcesToDownload(HtmlDocument htmlDoc, string currentPath, string baseUrl)
    {
        AddResourcesToDownload(htmlDoc, "//script[@src]", "src", currentPath, baseUrl);
        AddResourcesToDownload(htmlDoc, "//link[@rel='stylesheet']", "href", currentPath, baseUrl);
        AddResourcesToDownload(htmlDoc, "//img[@src]", "src", currentPath, baseUrl);
    }

    private void AddResourcesToDownload(
        HtmlDocument htmlDocument,
        string xpath,
        string attribute,
        string currentPath,
        string baseUrl)
    {
        var nodes = htmlDocument.DocumentNode.SelectNodes(xpath);
        if (nodes == null)
        {
            return;
        }

        foreach (var node in nodes)
        {
            var linkRelativeUrl = node.Attributes[attribute].Value;
            if (!Uri.IsWellFormedUriString(linkRelativeUrl, UriKind.Relative))
            {
                continue;
            }

            var absoluteUrl = UriHelper.CreateAbsoluteUri(baseUrl, currentPath, linkRelativeUrl).ToString();
            _resourcesToDownload.Add(absoluteUrl);
        }
    }

    private async Task SaveToFile(string data, Uri uri)
    {
        var path = CreateDirectoryForUri(uri);

        try
        {
            await File.WriteAllTextAsync(path, data);
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error saving content to file for {uri}: {e.Message}");
        }
    }

    private async Task DownloadFile(Uri uri)
    {
        Console.WriteLine($"Downloading {uri}");

        var path = CreateDirectoryForUri(uri);
        var data = await _httpClient.GetByteArrayAsync(uri);

        try
        {
            await File.WriteAllBytesAsync(path, data);
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error saving content to file for {uri}: {e.Message}");
        }
    }

    private async Task DownloadResources()
    {
        var tasks = new List<Task>();
        foreach (var resource in _resourcesToDownload)
        {
            tasks.Add(DownloadFile(new Uri(resource)));
        }
        await Task.WhenAll(tasks);
    }

    private string CreateDirectoryForUri(Uri uri)
    {
        var path = UriHelper.GetPath(uri, _configuration.BaseDirectory);
        var directoryName = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directoryName);
        return path;
    }
}