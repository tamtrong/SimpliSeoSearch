namespace SimpliSeoSearch.Core.Services;

public interface ISearchEngine
{
    string Name { get; }
    List<int> GetRankings(string keyword, string url);
}