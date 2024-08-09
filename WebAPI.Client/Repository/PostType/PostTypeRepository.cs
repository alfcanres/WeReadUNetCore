using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostType
{
    public class PostTypeRepository :
        BaseRepository<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>,
        IPostTypeRepository
    {
        public PostTypeRepository(IHttpClientHelper httpClientHelper)
            : base("api/posttype", httpClientHelper)
        {

        }

        public async Task<ResponseViewModel<int>> CountAllAsync()
        {
            return await HttpClientHelper.GetResponse<int>(HttpVerbsEnum.GET, $"/countall");
        }

        public async Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await HttpClientHelper.GetResponse<ResponseList<PostTypeReadDTO>, bool>(isAvailable, HttpVerbsEnum.GET, $"/isavailable/{isAvailable}");
        }

        public async Task<ResponseViewModel<ResponsePagedList<PostTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostTypeReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetTopTenAsync(int top)
        {
            return await HttpClientHelper.GetResponse<ResponseList<PostTypeReadDTO>, int>(10, HttpVerbsEnum.GET, "/top/10");
        }
    }
}
