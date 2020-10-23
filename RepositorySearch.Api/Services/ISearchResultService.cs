using System.Collections.Generic;
using System.Threading.Tasks;
using RepositorySearch.Api.Models;
using RepositorySearch.Api.ViewModels;

namespace RepositorySearch.Api.Services
{
    public interface ISearchResultService
    {
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<SearchResult>> GetResultsAsync(SearchQueryViewModel query);
        Task<int> GetTotalCountWithPermissionsAsync();
    }
}