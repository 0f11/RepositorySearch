using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositorySearch.Api.Services;
using RepositorySearch.Api.ViewModels;

namespace RepositorySearch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private ISearchResultService _searchResultService;


        public SearchController(ISearchResultService searchResultService)
        {
            _searchResultService = searchResultService;
        }

        [HttpGet]
        [Route("totalcount")]
        public async Task<int> GetTotalCountAsync()
        {
            return await _searchResultService.GetTotalCountAsync();
        }

        [HttpGet]
        [Route("count")]
        public async Task<int> GetTotalCountWithPermissionsAsync()
        {
            return await _searchResultService.GetTotalCountWithPermissionsAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResultViewModel>> GetSearchResultsAsync(
            [FromQuery] SearchQueryViewModel searchQuery)
        {
            var totalResultList = _searchResultService.GetResultsAsync(searchQuery).Result
                .Select(a => new SearchResultViewModel()
                {
                    Content = a.Content,
                    CreatedAt = a.Created,
                    UpdatedAt = a.Updated
                });
            return await Task.FromResult(totalResultList);
        }
    }
}