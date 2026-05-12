using System.Net.Http.Json;

namespace MYTEAMMANAGER.Services
{

    public class AzureFunctionsService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AzureFunctionsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task PassWelcomeEmail()
        {
            var url = _config["FunctionSettings:Url"] + "/api/PassWelcomeEmail";
            var response = await _httpClient.PostAsJsonAsync(
                url,
                new
                {
                    name = "Anandhu"
                }
            );
            response.EnsureSuccessStatusCode();
            
        }

    }
}