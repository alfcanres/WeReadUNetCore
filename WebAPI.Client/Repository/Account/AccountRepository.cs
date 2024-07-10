using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private const string _baseEndpoint = "api/account";
        private readonly string _clientName;
        private readonly HttpClient _client;
        private const string _loginEndpoint = "/login";
        private const string _registerEndpoint = "/register";
        private const string _changePasswordEndPoint = "/changepassword";
        private readonly ILogger<AccountRepository> _logger;


        public AccountRepository(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AccountRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _clientName = _configuration["HttpClientName"];
            _logger = logger;
            _client = _httpClientFactory.CreateClient(_clientName);

        }

        public async Task<ResponseViewModel<TokenResponse>> LoginAsync(UserSignInDTO loginViewModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            ResponseViewModel<TokenResponse> responseView = new ResponseViewModel<TokenResponse>();

            try
            {
                var response = await _client.PostAsync($"{_baseEndpoint}{_loginEndpoint}", content);


                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    responseView.Content = tokenResponse;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );

                    responseView.Validate = validateResponse;
                }
            }
            catch (Exception ex)
            {
                ValidatorResponse validateDTO = new ValidatorResponse();
                validateDTO.IsValid = false;
                validateDTO.MessageList = new List<string>() { ex.Message };
                responseView.Validate = validateDTO;
                _logger.LogError(ex, "An error occurred while processing the request.");
            }

            return responseView;

        }
        public async Task<ResponseViewModel<bool>> RegisterAsync(UserCreateDTO createModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(createModel), Encoding.UTF8, "application/json");
            var responseViewModel = new ResponseViewModel<bool>();

            try
            {
                var response = await _client.PostAsync($"{_baseEndpoint}{_registerEndpoint}", content);

                if (response.IsSuccessStatusCode)
                {
                    var userRegistered = await JsonSerializer.DeserializeAsync<UserRegisteredDTO>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );

                    responseViewModel.Content = true;
                    responseViewModel.Validate = userRegistered.ValidateDTO;
                }
                else
                {

                    var validateDTO = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        );
                    responseViewModel.Content = false;
                    responseViewModel.Validate = validateDTO;
                }

            }
            catch (Exception ex)
            {
                ValidatorResponse validateResponse = new ValidatorResponse();
                validateResponse.IsValid = false;
                validateResponse.MessageList = new List<string>() { ex.Message };
                responseViewModel.Validate = validateResponse;
                _logger.LogError(ex, "An error occurred while processing the request.");
            }

            return responseViewModel;
        }
    }
}
