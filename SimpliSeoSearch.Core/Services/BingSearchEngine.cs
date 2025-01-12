using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using SimpliSeoSearch.Core.Constants;

namespace SimpliSeoSearch.Core.Services;

public class BingSearchEngine : ISearchEngine
{
    private const string BingSearchUrl = "https://www.bing.com";
    private const string RegexPattern = $@"<cite>https:\/\/(.*?)\""";
    private const int PageSize = 10;

    private readonly IHttpClientFactory _httpClientFactory;

    public BingSearchEngine(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory  = httpClientFactory;
    }
    public string Name => "Bing Search";

    public List<int> GetRankings(string keyword, string url)
    {
        var totalPages = CommonConstants.SearchResultLimit / PageSize;
        var htmlContent = new StringBuilder();
        for (var pageNumber = 0; pageNumber < totalPages; pageNumber++)
        {
            var searchUrl = $"{BingSearchUrl}/search?q={Uri.EscapeDataString(keyword)}&first={pageNumber * PageSize + 1}";
            htmlContent.Append(FetchHtml(searchUrl));
        }
        
        return ParseRankings(htmlContent.ToString(), url, CommonConstants.SearchResultLimit);
    }

    private string FetchHtml(string url)
    {
        using var client = _httpClientFactory.CreateClient(nameof(GoogleSearchEngine));
        client.DefaultRequestHeaders.UserAgent.ParseAdd(CommonConstants.DefaultUserAgent);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

        try
        {
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching HTML: {ex.Message}");
            return string.Empty;
        }
    }

    private List<int> ParseRankings(string html, string url, int limit)
    {
        var rankings = new List<int>();
        var matches = Regex.Matches(html, RegexPattern).Take(limit);

        int rank = 1;
        foreach (Match match in matches)
        {
            if (match.ToString().Contains(url, StringComparison.OrdinalIgnoreCase))
            {
                rankings.Add(rank);
            }

            rank++;
        }

        return rankings;
    }
}