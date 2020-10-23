using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositorySearch.Api.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public Task<IEnumerable<string>> GetUserGroups()
        {
            return Task.FromResult<IEnumerable<string>>(new[] {"developers"});
        }

        public Task<IEnumerable<string>> GetUserGroups(string userName)
        {
            return Task.FromResult<IEnumerable<string>>(new[] {"developers"});
        }
    }
}