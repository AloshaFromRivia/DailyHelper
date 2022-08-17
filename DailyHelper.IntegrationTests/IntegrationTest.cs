using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DailyHelper.Models;
using DailyHelper.Models.ViewModels.Requests;
using DailyHelper.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DailyHelper.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(ser =>
                    {
                        ser.RemoveAll(typeof(ApplicationDbContext));
                        ser.AddDbContext<ApplicationDbContext>(optionsBuilder =>
                        {
                            optionsBuilder.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response =
                await TestClient.PostAsJsonAsync("api/account/register",
                    new UserRegistrationRequest()
                    {
                        Email = "user@testexample.com",
                        Password = "Secret123",
                        Name = "Alex"
                    });
            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
            return registrationResponse?.Token;
        }
    }
}