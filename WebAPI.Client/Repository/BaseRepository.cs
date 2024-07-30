using DataTransferObjects;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace WebAPI.Client.Repository
{
    public class BaseRepository<CreateDTO, ReadDTO, UpdateDTO>
        where ReadDTO : class
        where CreateDTO : class
        where UpdateDTO : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _baseEndpoint;
        private readonly string _clientName;
        private readonly HttpClient _client;
        private readonly ILogger<BaseRepository<CreateDTO, ReadDTO, UpdateDTO>> _logger;
        private const string _refreshTokenEndpoint = "/api/Account/RefreshToken";
        private string _token = string.Empty;

        public HttpClient Client => _client;

        public string BaseEndpoint => _baseEndpoint;

        public BaseRepository(
            string baseEndPoint,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BaseRepository<CreateDTO, ReadDTO, UpdateDTO>> logger
            )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _clientName = _configuration["HttpClientName"];
            _baseEndpoint = baseEndPoint;
            _client = _httpClientFactory.CreateClient(_clientName);
            _logger = logger;
        }

        public void SetBearerToken(string bearerToken)
        {


            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                _client.DefaultRequestHeaders.Authorization = null;
                _token = null;
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                _token = bearerToken;

            }
        }

        private async Task<bool> RefreshTokenAsync()
        {
            if (String.IsNullOrEmpty(_token))
            {
                return false;
            }

            var response = await _client.PostAsync(_refreshTokenEndpoint, new StringContent(_token));

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                SetBearerToken(tokenResponse.Token);
                return true;
            }

            return false;
        }

        public async Task<ResponseViewModel<ReadDTO>> CreateAsync(CreateDTO createModel)
        {
            return await GetResponse<ReadDTO, CreateDTO>(createModel, HttpVerbsEnum.POST);
        }

        public async Task<ResponseViewModel<ReadDTO>> UpdateAsync(UpdateDTO createModel)
        {
            return await GetResponse<ReadDTO, UpdateDTO>(createModel, HttpVerbsEnum.PUT);
        }

        public async Task<ResponseViewModel<ReadDTO>> GetByIdAsync(int id)
        {
            return await GetResponse<ReadDTO, int>(id, HttpVerbsEnum.GET, $"/{id}");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            return await GetValidateResponse(id, HttpVerbsEnum.DELETE, $"/{id}");
        }

        protected async Task<ResponseViewModel<RetDTO>> GetResponse<RetDTO, InputDTO>(InputDTO inputDTO, HttpVerbsEnum HttpVerb, string endPoint = "")
        {
            var content = new StringContent(JsonSerializer.Serialize(inputDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            ResponseViewModel<RetDTO> responseView = new ResponseViewModel<RetDTO>();


            try
            {

                response = await SendRequest(HttpVerb, content, endPoint);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenAsync())
                    {
                        response = await SendRequest(HttpVerb, content, endPoint);
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseDTO = await JsonSerializer.DeserializeAsync<RetDTO>(await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    responseView.Content = responseDTO;
                    responseView.Status = ResponseStatus.Success;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ValidatorResponse validateDTO = new ValidatorResponse();
                    validateDTO.IsValid = false;
                    validateDTO.MessageList = new List<string>() { "Unauthorized" };
                    responseView.Status = ResponseStatus.Unauthorized;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    responseView.Status = ResponseStatus.Error;
                    responseView.MessageList = validateResponse.MessageList;
                }
            }
            catch (Exception ex)
            {

                responseView.Status = ResponseStatus.Error;
                responseView.MessageList = new List<string>() { "An error occurred while processing the request." };
                _logger.LogError(ex, $"EndPoint : {endPoint} | HttpVerb : {HttpVerb} | Input Data: {inputDTO}");
            }


            return responseView;

        }

        protected async Task<ResponseViewModel<bool>> GetValidateResponse<T>(T input, HttpVerbsEnum HttpVerb, string endPoint = "")
        {
            var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            try
            {

                response = await SendRequest(HttpVerb, content, endPoint);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenAsync())
                    {
                        response = await SendRequest(HttpVerb, content, endPoint);
                    }
                }


                if (response.IsSuccessStatusCode)
                {
                    return new ResponseViewModel<bool>() { Content = true, Status = ResponseStatus.Success };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new ResponseViewModel<bool>() { Content = false, Status = ResponseStatus.Unauthorized };
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return new ResponseViewModel<bool>() { Content = false, Status = ResponseStatus.Error, MessageList = validateResponse.MessageList };
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"EndPoint : {endPoint} | HttpVerb : {HttpVerb} | Input Data: {input}");
                return new ResponseViewModel<bool>()
                {
                    Content = false,
                    Status = ResponseStatus.Error,
                    MessageList = new List<string>() { "An error occurred while processing the request." }
                };
            }

        }

        private async Task<HttpResponseMessage> SendRequest(HttpVerbsEnum HttpVerb, HttpContent content, string endPoint)
        {
            HttpResponseMessage response = null;

            switch (HttpVerb)
            {
                case HttpVerbsEnum.GET:
                    response = await _client.GetAsync($"{BaseEndpoint}{endPoint}");
                    break;
                case HttpVerbsEnum.POST:
                    response = await _client.PostAsync($"{BaseEndpoint}{endPoint}", content);
                    break;
                case HttpVerbsEnum.PUT:
                    response = await _client.PutAsync($"{BaseEndpoint}{endPoint}", content);
                    break;
                case HttpVerbsEnum.DELETE:
                    response = await _client.DeleteAsync($"{BaseEndpoint}{endPoint}");
                    break;
            }

            return response;
        }
    }
}
