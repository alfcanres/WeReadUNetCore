using DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private const string _refreshTokenEndpoint = "/api/Account/RefreshToken";
        private string _token = string.Empty;
        private readonly ILogger<HttpClientHelper> _logger;

        public HttpClientHelper(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<HttpClientHelper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            string _clientName = _configuration["HttpClientName"];
            _client = _httpClientFactory.CreateClient(_clientName);
        }

        public string Token { get => _token; }

        public void SetBearerToken(string bearerToken)
        {


            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                _client.DefaultRequestHeaders.Authorization = null;
                _token = string.Empty;
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                _token = bearerToken;

            }
        }
        public async Task<ResponseViewModel<RetDTO>> GetResponse<RetDTO, InputDTO>(InputDTO inputDTO, HttpVerbsEnum HttpVerb, string endPoint = "")
        {
            var content = new StringContent(JsonSerializer.Serialize(inputDTO), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            ResponseViewModel<RetDTO> responseView = new ResponseViewModel<RetDTO>();


            try
            {

                response = await SendRequest(HttpVerb, endPoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenAsync())
                    {
                        response = await SendRequest(HttpVerb, endPoint, content);
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
        public async Task<ResponseViewModel<bool>> GetValidateResponse<T>(T input, HttpVerbsEnum HttpVerb, string endPoint = "")
        {
            var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            try
            {

                response = await SendRequest(HttpVerb, endPoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenAsync())
                    {
                        response = await SendRequest(HttpVerb, endPoint, content);
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
        public async Task<ResponseViewModel<RetDTO>> GetResponse<RetDTO>(HttpVerbsEnum HttpVerb, string endPoint = "")
        {


            HttpResponseMessage response = null;

            ResponseViewModel<RetDTO> responseView = new ResponseViewModel<RetDTO>();


            try
            {

                response = await SendRequest(HttpVerb, endPoint);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (await RefreshTokenAsync())
                    {
                        response = await SendRequest(HttpVerb, endPoint);
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
                _logger.LogError(ex, $"EndPoint : {endPoint} | HttpVerb : {HttpVerb}");
            }


            return responseView;

        }

        private async Task<bool> RefreshTokenAsync()
        {
            if (String.IsNullOrEmpty(Token))
            {
                return false;
            }

            var response = await _client.PostAsync(_refreshTokenEndpoint, new StringContent(Token));

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                SetBearerToken(tokenResponse.Token);
                return true;
            }

            return false;
        }
        private async Task<HttpResponseMessage> SendRequest(HttpVerbsEnum HttpVerb, string endPoint, HttpContent? content = null)
        {
            HttpResponseMessage response = null;

            switch (HttpVerb)
            {
                case HttpVerbsEnum.GET:
                    response = await _client.GetAsync(endPoint);
                    break;
                case HttpVerbsEnum.POST:
                    response = await _client.PostAsync($"{endPoint}", content);
                    break;
                case HttpVerbsEnum.PUT:
                    response = await _client.PutAsync($"{endPoint}", content);
                    break;
                case HttpVerbsEnum.DELETE:
                    response = await _client.DeleteAsync($"{endPoint}");
                    break;
            }

            return response;
        }

        
    }
}
