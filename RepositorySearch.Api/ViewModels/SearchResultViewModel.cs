using System;

namespace RepositorySearch.Api.ViewModels
{
    public class SearchResultViewModel
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Content { get; set; }
    }
}