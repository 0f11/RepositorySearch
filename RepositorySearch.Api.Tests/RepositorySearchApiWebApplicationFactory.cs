using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RepositorySearch.Api.Services;

namespace RepositorySearch.Api.Tests
{
    public class RepositorySearchApiWebApplicationFactory : WebApplicationFactory<RepositorySearch.Api.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IAuthorizationService, AuthorizationServiceStub>();
            });
        }
    }
}