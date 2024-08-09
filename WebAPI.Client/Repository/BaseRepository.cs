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
        private readonly IHttpClientHelper _httpClientHelper;

        //private readonly string _clientName;
        //private readonly HttpClient _client;
        //public HttpClient Client => _client;

        public IHttpClientHelper HttpClientHelper => _httpClientHelper;

        public BaseRepository(
            string baseEndPoint,
            IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
            _httpClientHelper.BaseEndpoint = baseEndPoint;
        }

        public void SetBearerToken(string bearerToken)
        {
            HttpClientHelper.SetBearerToken(bearerToken);
        }


        public async Task<ResponseViewModel<ReadDTO>> CreateAsync(CreateDTO createModel)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, CreateDTO>(createModel, HttpVerbsEnum.POST);
        }

        public async Task<ResponseViewModel<ReadDTO>> UpdateAsync(UpdateDTO createModel)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, UpdateDTO>(createModel, HttpVerbsEnum.PUT);
        }

        public async Task<ResponseViewModel<ReadDTO>> GetByIdAsync(int id)
        {
            return await HttpClientHelper.GetResponse<ReadDTO, int>(id, HttpVerbsEnum.GET, $"/{id}");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            return await HttpClientHelper.GetValidateResponse(id, HttpVerbsEnum.DELETE, $"/{id}");
        }


    }
}
