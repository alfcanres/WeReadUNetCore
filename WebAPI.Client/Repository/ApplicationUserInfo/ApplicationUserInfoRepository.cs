using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.Helpers;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.ApplicationUserInfo
{
    public class ApplicationUserInfoRepository :
        BaseRepository<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>,
        IApplicationUserInfoRepository
    {
        public ApplicationUserInfoRepository(
                   IHttpClientFactory httpClientFactory,
                   IConfiguration configuration,
                   ILogger<BaseRepository<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>> logger
                   ) :
                   base(
                       "api/applicationuserinfo",
                       httpClientFactory,
                       configuration,
                       logger
                       )
        {

        }

        public Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoReadDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<ApplicationUserInfoReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }
        public Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoReadDTO>>> GetPagedByActiveAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<ApplicationUserInfoReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }
        public Task<ResponseViewModel<ResponseList<ApplicationUserInfoReadDTO>>> GetTopTenAsync()
        {
            return GetResponse<ResponseList<ApplicationUserInfoReadDTO>, int>(10, HttpVerbsEnum.GET, "/top/10");
        }
    }
}
