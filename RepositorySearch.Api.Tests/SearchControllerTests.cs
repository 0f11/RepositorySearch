using System;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;
using RepositorySearch.Api.ViewModels;
using System.Linq;

namespace RepositorySearch.Api.Tests
{
    public class SearchControllerTests : IClassFixture<RepositorySearchApiWebApplicationFactory>
    {
        HttpClient _client { get; }

        public SearchControllerTests(RepositorySearchApiWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Correct_total_count_of_documents_is_returned()
        {
            var response = await _client.GetAsync("/api/search/totalcount");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultsCount = await JsonSerializer.DeserializeAsync<int>(
                await response.Content.ReadAsStreamAsync());

            resultsCount.Should().Be(10);
        }

        [Fact]
        public async Task Permissions_are_checked_for_total_count_of_documents()
        {
            var response = await _client.GetAsync("/api/search/count");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultsCount = await JsonSerializer.DeserializeAsync<int>(
                await response.Content.ReadAsStreamAsync());

            resultsCount.Should().Be(5);
        }

        [Fact]
        public async Task Query_works_with_permission_checking()
        {
            var response = await _client.GetAsync("/api/search?query=configuration");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await JsonSerializer.DeserializeAsync<IEnumerable<SearchResultViewModel>>(
                await response.Content.ReadAsStreamAsync());

            results.Should().HaveCount(2);
        }

        [Fact]
        public async Task Query_is_case_insensitive()
        {
            var response = await _client.GetAsync("/api/search?query=git");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await JsonSerializer.DeserializeAsync<IEnumerable<SearchResultViewModel>>(
                await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            results.Should().HaveCount(3);
            results.Should().OnlyContain(x => x.Content.Contains("git", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Pagination_has_one_result_on_the_last_page()
        {
            var response = await _client.GetAsync("/api/search?query=to&skip=2&top=2");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await JsonSerializer.DeserializeAsync<IEnumerable<SearchResultViewModel>>(
                await response.Content.ReadAsStreamAsync());

            results.Should().HaveCount(1);
        }

        [Fact]
        public async Task Highlights_are_added_to_matching_strings()
        {
            var response = await _client.GetAsync("/api/search?query=GUID");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await JsonSerializer.DeserializeAsync<IEnumerable<SearchResultViewModel>>(
                await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            results.Should().HaveCount(1);
            //Typo in word highlight, I changed it in the sake of passing tests, hope its not a problem. - Rommi Parman
            results.First().Content.Should().Be(@"If you need to generate a <span class=""highlight"">GUID</span>, it is very easy to do in PowerShell. Just use `[<span class=""highlight"">guid</span>]::New<span class=""highlight"">Guid</span>()` for this.");
        }
    }
}
