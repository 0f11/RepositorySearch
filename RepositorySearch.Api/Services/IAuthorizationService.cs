using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositorySearch.Api.Services
{
    public interface IAuthorizationService
    {
        Task<IEnumerable<string>> GetUserGroups();
        Task<IEnumerable<string>> GetUserGroups(string userName);
    }
}