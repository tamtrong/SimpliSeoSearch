using SimpliSeoSearch.Core.Cache;
using SimpliSeoSearch.Core.Constants;

namespace SimpliSeoSearch.Core.Services;

public interface ISearchService
{
    public Dictionary<string, List<int>> Search(string keyword, string url, List<string> searchEnginesToUse);
}

public class SearchService : ISearchService
{
    private readonly ICacheService _cacheService;
    private readonly IEnumerable<ISearchEngine> _supportedSearchEngines;

    public SearchService(ICacheService cacheService, IEnumerable<ISearchEngine> supportedSearchEngines)
    {
        _cacheService = cacheService;
        _supportedSearchEngines = supportedSearchEngines;
    }

    public Dictionary<string, List<int>> Search(string keyword, string url, List<string> searchEnginesToUse)
    {
        var results = new Dictionary<string, List<int>>();
        foreach (var engine in searchEnginesToUse.Where(e => _supportedSearchEngines.Any(x => x.Name == e)))
        {
            var supportedEngine = _supportedSearchEngines.First(x => x.Name == engine);
            var cacheKey = $"{engine}_{keyword}_{url}";
            var cacheValue = _cacheService.Get(cacheKey) as List<int>;
            if (cacheValue != null)
            {
                results[engine] = cacheValue;
                continue;
            }

            var rankings = supportedEngine.GetRankings(keyword, url);
            rankings = rankings.Any() ? rankings : [0];
            _cacheService.Set(cacheKey, rankings, TimeSpan.FromMinutes(CommonConstants.CacheExpireInMinutes));
            results[engine] = rankings;
        }

        return results;
    }
}