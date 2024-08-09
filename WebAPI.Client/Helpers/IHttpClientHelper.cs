﻿using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Helpers
{
    public interface IHttpClientHelper
    {
        string BaseEndpoint { get; set; }
        string Token { get; }


        Task<ResponseViewModel<RetDTO>> GetResponse<RetDTO>(HttpVerbsEnum HttpVerb, string endPoint = "");
        Task<ResponseViewModel<RetDTO>> GetResponse<RetDTO, InputDTO>(InputDTO inputDTO, HttpVerbsEnum HttpVerb, string endPoint = "");
        Task<ResponseViewModel<bool>> GetValidateResponse<T>(T input, HttpVerbsEnum HttpVerb, string endPoint = "");
        void SetBearerToken(string bearerToken);
    }
}