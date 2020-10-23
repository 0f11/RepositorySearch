using System;

namespace RepositorySearch.Api.Models
{
    public class SearchResult
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public string Content { get; set; }
        public string[] Groups { get; set; }
    }
}