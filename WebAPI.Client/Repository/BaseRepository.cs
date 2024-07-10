using DataTransferObjects;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }
        }

        public async Task<ResponseViewModel<Response<ReadDTO>>> CreateAsync(CreateDTO createModel)
        {
            return await GetResponse<Response<ReadDTO>, CreateDTO>(createModel, HttpVerbsEnum.POST);
        }

        public async Task<ResponseViewModel<Response<ReadDTO>>> UpdateAsync(UpdateDTO createModel)
        {
            return await GetResponse<Response<ReadDTO>, UpdateDTO>(createModel, HttpVerbsEnum.PUT);
        }

        public async Task<ResponseViewModel<Response<ReadDTO>>> GetByIdAsync(int id)
        {
            return await GetResponse<Response<ReadDTO>, int>(id, HttpVerbsEnum.GET, $"/{id}");
        }

        public async Task<ValidatorResponse> DeleteAsync(int id)
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

                //responseView.StatusCode = response.StatusCode;

                string contentString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {


                    var responseDTO = await JsonSerializer.DeserializeAsync<RetDTO>(await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    responseView.Content = responseDTO;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ValidatorResponse validateDTO = new ValidatorResponse();
                    validateDTO.IsValid = false;
                    validateDTO.MessageList = new List<string>() { "Unauthorized" };
                    responseView.Validate = validateDTO;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

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

        protected async Task<ValidatorResponse> GetValidateResponse<T>(T input, HttpVerbsEnum HttpVerb, string endPoint = "")
        {
            var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            try
            {

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



                if (response.IsSuccessStatusCode)
                {
                    return new ValidatorResponse() { IsValid = true };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ValidatorResponse validateDTO = new ValidatorResponse();
                    validateDTO.IsValid = false;
                    validateDTO.MessageList = new List<string>() { "Unauthorized" };
                    return validateDTO;
                }
                else
                {
                    var validateResponse = await JsonSerializer.DeserializeAsync<ValidatorResponse>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return validateResponse;
                }
            }
            catch (Exception ex)
            {
                ValidatorResponse validateDTO = new ValidatorResponse();
                validateDTO.IsValid = false;
                validateDTO.MessageList = new List<string>() { ex.Message };
                _logger.LogError(ex, "An error occurred while processing the request.");
                return validateDTO;
            }

        }


    }
}
