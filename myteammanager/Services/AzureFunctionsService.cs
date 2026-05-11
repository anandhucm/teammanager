using System.Net.Http.Json;

namespace MYTEAMMANAGER.Services
{

    public class AzureFunctionsService
    {

        private readonly HttpClient _httpClient;

        public AzureFunctionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PassWelcomeEmail()
        {
            var url = "http://localhost:7071/api/PassWelcomeEmail";
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