using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using SimpliSeoSearch.Core.Constants;

namespace SimpliSeoSearch.Core.Services;

public class GoogleSearchEngine : ISearchEngine
{
    private const string GoogleSearchUrl = $"https://www.google.com";
    private const string RegexPattern = @"data-id=""atritem-https:\/\/(.*?)\""";
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleSearchEngine(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory  = httpClientFactory;
    }
    public string Name => "Google Search";

    public List<int> GetRankings(string keyword, string url)
    {
        var searchUrl = $"{GoogleSearchUrl}/search?q={Uri.EscapeDataString(keyword)}&num={CommonConstants.SearchResultLimit}";
        var htmlContent = FetchHtml(searchUrl);
        return ParseRankings(htmlContent, url, CommonConstants.SearchResultLimit);
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