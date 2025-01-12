namespace SimpliSeoSearch.Core.Models;

public class SearchSeoRequest
{
    public string Keyword { get; set; }
    public string Url { get; set; }
    public List<string> SearchEngines { get; set; }
}