using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SimpliSeoSearch.API.Controllers;
using SimpliSeoSearch.Core.Models;
using SimpliSeoSearch.Core.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SympliSeoSearch.UnitTest.Controllers;

public class SearchControllerTests
{
    [Fact]
    public void When_Search_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var mockService = new Mock<ISearchService>();
        mockService.Setup(s => s.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()))
            .Returns(new Dictionary<string, List<int>> { { "Google Search", new List<int> { 3, 17 } } });

        var mockLogger = new Mock<ILogger<SearchSeoController>>();
        var controller = new SearchSeoController(mockService.Object, mockLogger.Object);
        var request = new SearchSeoRequest
        {
            Keyword = "e-settlements",
            Url = "www.sympli.com.au",
            SearchEngines = new List<string> { "Google Search" }
        };

        // Act
        var result = controller.Search(request) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        var data = Assert.IsType<Dictionary<string, List<int>>>(result.Value);
        var googleSearchData = data["Google Search"];
        Assert.NotNull(googleSearchData);
        Assert.Equal(2, googleSearchData.Count);
        Assert.Contains(3, googleSearchData);
        Assert.Contains(17, googleSearchData);
    }

    [Fact]
    public void When_Search_MissingKeywordOrUrl_ReturnsBadRequest()
    {
        // Arrange
        var mockService = new Mock<ISearchService>();
        var mockLogger = new Mock<ILogger<SearchSeoController>>();
        var controller = new SearchSeoController(mockService.Object, mockLogger.Object);
        var request = new SearchSeoRequest { Keyword = "", Url = "" };

        // Act
        var result = controller.Search(request) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Keyword and URL are required.", result.Value);
    }
}