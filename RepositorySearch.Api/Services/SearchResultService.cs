using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RepositorySearch.Api.Models;
using RepositorySearch.Api.ViewModels;

namespace RepositorySearch.Api.Services
{
    public class SearchResultService : ISearchResultService
    {
        private const string Path =
            @"../RepositorySearch.Api/Resources/data.json";

        private IAuthorizationService _authService;
        private SearchResult[] _ctx;
        private string[] _permissions;

        
        
        
        public SearchResultService(IAuthorizationService authorizationService)
        {
            _authService = authorizationService;
            _ctx = JsonConvert.DeserializeObject<SearchResult[]>(File.ReadAllText(Path));
            _permissions = _authService.GetUserGroups().Result.ToArray();
        }

        public async Task<int> GetTotalCountAsync()
        {
            //Should be moved to DAl? Or to private variable?
            //SearchResult[] searchResults = JsonConvert.DeserializeObject<SearchResult[]>(File.ReadAllText(Path));

            return await Task.FromResult(_ctx.Length);
        }

        public async Task<int> GetTotalCountWithPermissionsAsync()
        {
            //Can one user have multiple permissions?
            // SearchResult[] searchResults = JsonConvert.DeserializeObject<SearchResult[]>(File.ReadAllText(Path))
            //     .Where(r => r.Groups.Contains(permission, StringComparer.CurrentCultureIgnoreCase))
            //     .ToArray();
            SearchResult[] searchResults = _ctx
                .Where(r => r.Groups.Any(p => _permissions.Any(y => y == p))).ToArray();


            //var totalPermissionsList = searchResults.Where(r => r.Groups.Contains(permission)).ToArray();

            return await Task.FromResult(searchResults.Length);
        }


        public async Task<IEnumerable<SearchResult>> GetResultsAsync(SearchQueryViewModel query)
        {
            var searchString = Regex.Escape(query.Query);

            var start = @"<span class=""highlight"">";
            var end = @"</span>";

            var re = new Regex(@"(" + searchString + @")\b", RegexOptions.IgnoreCase);

            //What if query string = "to" ? Shall regex be applied? Or how to distinguish substrings from strings with LINQ query? 
            //Solution provided is problematic, i.e Content containing string [guid] or any other special chars.

            // var totalResultList = searchResults.Where(
            //     r => r.Content
            //     .IndexOf(
            //         query.Query, StringComparison.OrdinalIgnoreCase) != -1).ToArray();
            //var totalResultList = searchResults.Where(r => r.Content.Contains(query.Query.ToLower())).ToArray();
            //@"(?<TM>\w*TEST\w*)"

            var searchResults = _ctx
                .Where(r =>
                    r.Content.Split().Any(w => re.IsMatch(w)) &&
                    r.Groups.Any(p => _permissions.Any(y => y == p)))
                .ToArray();

            //Regex.Replace(r.Content, @"(?<!>)(" + m + @")\b", start + m + end)
            //r.Content.Split().Any(s=> Regex.Replace(r.Content, @"(?<!>)(" + s + @")\b", start+s+end))
            //Really expensive solution, should be somehow implemented in LINQ query where result is originally filtered...
            //Should NewGuid be highlighted i.e if to in administrator is not? Regex groups?
            foreach (var searchResult in searchResults)
            {
                var match = re.Matches(searchResult.Content);
                foreach (Match m in match)
                {
                    searchResult.Content =
                        Regex.Replace(searchResult.Content, @"(?<!>)(" + m + @")\b", start + m + end);
                }

                // var dummy = searchResult.Content.Split();
                //
                // for (var i = 0; i < dummy.Length; i++)
                // {
                //     var match = re.Match(dummy[i]);
                //
                //     if (match.Success)
                //     {
                //         dummy[i] = dummy[i].Replace(match.Groups[1].Value, start + match.Groups[1].Value + end,
                //             StringComparison.CurrentCultureIgnoreCase);
                //         //dummy[i] = @"<span class=""highlight"">" + dummy[i] + @"</span>";
                //         //Regex.Replace(dummy[i], re.ToString(), start +  + end);
                //     }
                // }
                //
                // searchResult.Content = string.Join(" ", dummy);
            }

            //Cannot skip without top & vice versa
            var queryResult = searchResults
                .Skip(query.Top * ((int) Math.Ceiling((double) searchResults.Length / query.Top) - 1))
                .Take(query.Top > 0 ? query.Top : searchResults.Length).ToArray();

            return await Task.FromResult(queryResult);
        }
    }
}