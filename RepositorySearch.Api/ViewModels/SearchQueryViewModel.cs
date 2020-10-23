namespace RepositorySearch.Api.ViewModels
{
    public class SearchQueryViewModel
    {
        public string Query { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
    }
}