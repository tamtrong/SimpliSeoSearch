using Moq;
using SimpliSeoSearch.Core.Cache;
using SimpliSeoSearch.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SympliSeoSearch.UnitTest.Services;

public class SearchServiceTests
{
    [Fact]
    public void When_Search_ValidInput_ReturnsResults()
    {
        // Arrange
        var googleMock = new Mock<ISearchEngine>();
        googleMock.Setup(g => g.Name).Returns("Google Search");
        googleMock.Setup(g => g.GetRankings(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new List<int> { 3, 17 });

        var bingMock = new Mock<ISearchEngine>();
        bingMock.Setup(b => b.Name).Returns("Bing Search");
        bingMock.Setup(b => b.GetRankings(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new List<int> { 2, 13, 24, 66 });

        var cacheServiceMock = new Mock<ICacheService>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        var searchService = new SearchService(cacheServiceMock.Object, new[] { googleMock.Object, bingMock.Object });

        // Act
        var results = searchService.Search("keyword", "url", ["Google Search", "Bing Search"]);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        var data = Assert.IsType<Dictionary<string, List<int>>>(results);
        var googleSearchData = data["Google Search"];
        Assert.NotNull(googleSearchData);
        Assert.Equal(2, googleSearchData.Count);
        Assert.Contains(3, googleSearchData);
        Assert.Contains(17, googleSearchData);

        var bingSearchData = data["Bing Search"];
        Assert.NotNull(bingSearchData);
        Assert.Equal(4, bingSearchData.Count);
        Assert.Contains(2, bingSearchData);
        Assert.Contains(13, bingSearchData);
        Assert.Contains(24, bingSearchData);
        Assert.Contains(66, bingSearchData);
    }

    [Fact]
    public void When_Search_CacheHit_ReturnsCachedResults()
    {
        // Arrange
        var cacheServiceMock = new Mock<ICacheService>();
        cacheServiceMock.Setup(c => c.Get(It.IsAny<string>()))
            .Returns(new List<int> { 3, 17 });

        var googleMock = new Mock<ISearchEngine>();
        googleMock.Setup(g => g.Name).Returns("Google Search");

        var bingMock = new Mock<ISearchEngine>();
        bingMock.Setup(b => b.Name).Returns("Bing Search");

        var searchService = new SearchService(cacheServiceMock.Object, [googleMock.Object, bingMock.Object]);

        // Act
        var results = searchService.Search("keyword", "url", ["Google Search"]);

        // Assert
        Assert.NotNull(results);
        var data = Assert.IsType<Dictionary<string, List<int>>>(results);
        var googleSearchData = data["Google Search"];
        Assert.NotNull(googleSearchData);
        Assert.Equal(2, googleSearchData.Count);
        Assert.Contains(3, googleSearchData);
        Assert.Contains(17, googleSearchData);
        cacheServiceMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
        cacheServiceMock.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<int>>(), It.IsAny<TimeSpan>()),
            Times.Never);
    }

    [Fact]
    public void When_Search_CacheMiss_SavesToCache()
    {
        // Arrange
        var cacheServiceMock = new Mock<ICacheService>();
        cacheServiceMock.Setup(c => c.Get(It.IsAny<string>()))
            .Returns(null as List<int>);

        var googleMock = new Mock<ISearchEngine>();
        googleMock.Setup(g => g.Name).Returns("Google Search");
        googleMock.Setup(g => g.GetRankings(It.IsAny<string>(), It.IsAny<string>()))
            .Returns([3, 17]);

        var bingMock = new Mock<ISearchEngine>();
        bingMock.Setup(b => b.Name).Returns("Bing Search");
        bingMock.Setup(b => b.GetRankings(It.IsAny<string>(), It.IsAny<string>()))
            .Returns([2, 13, 24, 66]);

        var searchService = new SearchService(cacheServiceMock.Object, [googleMock.Object, bingMock.Object]);

        // Act
        var results = searchService.Search("keyword", "url", new List<string>() { "Google Search" });

        // Assert
        Assert.NotNull(results);
        Assert.Single(results);
        cacheServiceMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
        cacheServiceMock.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<int>>(), It.IsAny<TimeSpan>()),
            Times.Once);
    }
}