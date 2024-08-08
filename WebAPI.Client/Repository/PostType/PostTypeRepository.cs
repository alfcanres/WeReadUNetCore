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
        public PostTypeRepository(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<BaseRepository<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>> logger
        ) :
        base(
        "api/posttype",
        httpClientFactory,
        configuration,
        logger
        )
        {

        }

        public Task<int> CountAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return GetResponse<ResponseList<PostTypeReadDTO>, bool>(isAvailable, HttpVerbsEnum.GET, $"/isavailable/{isAvailable}");
        }

        public Task<ResponseViewModel<ResponsePagedList<PostTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<PostTypeReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<ResponseList<PostTypeReadDTO>>> GetTopTenAsync(int top)
        {
            return GetResponse<ResponseList<PostTypeReadDTO>, int>(10, HttpVerbsEnum.GET, "/top/10");
        }
    }
}
