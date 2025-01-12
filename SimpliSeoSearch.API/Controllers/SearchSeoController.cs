using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SimpliSeoSearch.Core.Models;
using SimpliSeoSearch.Core.Services;

namespace SimpliSeoSearch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchSeoController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<SearchSeoController> _logger;
    
    public SearchSeoController(ISearchService searchService, ILogger<SearchSeoController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("ok");
    }
    
    [HttpPost]
    [Route("Search")]
    public IActionResult Search([FromBody] SearchSeoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword) || string.IsNullOrWhiteSpace(request.Url))
        {
            return BadRequest("Keyword and URL are required.");
        }

        var results = _searchService.Search(request.Keyword, request.Url, request.SearchEngines);
        return Ok(results);
    }
}