using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Repository
{
    public class AccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private const string _baseEndpoint = "api/account";
        private readonly string _clientName;

        public AccountRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _clientName = _configuration["HttpClientName"];
        }

        public async Task<TokenResponseViewModel> Login(UserSignInDTO loginViewModel)
        {
            var client = _httpClientFactory.CreateClient(_clientName);
            var content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseEndpoint}/login", content);


            var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponseViewModel>(
               await response.Content.ReadAsStreamAsync(),
                                  new JsonSerializerOptions
                                  {
                                      PropertyNameCaseInsensitive = true
                                  });

            return tokenResponse;


        }

        public async Task<IResponseDTO<UserReadDTO>> Register(UserCreateDTO createModel)
        {
            var client = _httpClientFactory.CreateClient(_clientName);
            var content = new StringContent(JsonSerializer.Serialize(createModel), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{_baseEndpoint}/register", content);

            var responseDTO = await JsonSerializer.DeserializeAsync<IResponseDTO<UserReadDTO>>(
                                  await response.Content.ReadAsStreamAsync(),
                                  new JsonSerializerOptions
                                  {
                                      PropertyNameCaseInsensitive = true
                                  });

            return responseDTO;


        }

        public async Task<IResponseDTO<UserReadDTO>> ChangePassword(UserUpdatePasswordDTO updateModel)
        {
            var client = _httpClientFactory.CreateClient(_clientName);
            var content = new StringContent(JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_baseEndpoint}/changepassword", content);

            if (
                               response.IsSuccessStatusCode
                                              ||
                                                             response.StatusCode == System.Net.HttpStatusCode.BadRequest
                                                                            )
            {
                var responseDTO = await JsonSerializer.DeserializeAsync<IResponseDTO<UserReadDTO>>(
                                                         await response.Content.ReadAsStreamAsync(),
                                                                                              new JsonSerializerOptions
                                                                                              {
                                                                                                  PropertyNameCaseInsensitive = true
                                                                                              });

                return responseDTO;
            }
            else
            {
                return null;
            }

        }
    }
}
