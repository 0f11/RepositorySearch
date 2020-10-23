using System.Collections.Generic;
using System.Threading.Tasks;
using RepositorySearch.Api.Services;

namespace RepositorySearch.Api.Tests
{
    internal class AuthorizationServiceStub : IAuthorizationService
    {
        public Task<IEnumerable<string>> GetUserGroups()
        {
            return Task.FromResult<IEnumerable<string>>(new[] { "developers" });
        }

        public Task<IEnumerable<string>> GetUserGroups(string userName)
        {
            return Task.FromResult<IEnumerable<string>>(new[] { "developers" });
        }
    }
}