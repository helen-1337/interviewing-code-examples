namespace WebScraping;

public static class UriHelper
{
    public static Uri CreateAbsoluteUri(string baseUrl, string currentPath, string relativeUrl)
    {
        return new Uri(new Uri(baseUrl + currentPath), relativeUrl);
    }

    public static string GetPath(Uri uri, string baseDirectory)
    {
        var filePath = uri.AbsolutePath;
        if (filePath == "/")
        {
            filePath = $"{uri.Host}.html";
        }
        var fixImagePath = Path.Combine(filePath.Split("/"));
        var combinedPath = Path.Combine(baseDirectory, fixImagePath);
        return combinedPath;
    }

    public static string GetRelativePathWithoutFileName(Uri uri)
    {
        return string.Join("", uri.Segments.Take(uri.Segments.Length - 1));
    }
}