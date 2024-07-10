using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.MoodType
{
    public class MoodTypeRepository :
        BaseRepository<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>,
        IMoodTypeRepository
    {

        public MoodTypeRepository(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BaseRepository<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>> logger
            ) :
            base(
                "api/moodtype",
                httpClientFactory,
                configuration,
                logger
                )
        {

        }

        public Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetIsAvailableAsync(bool isAvailable)
        {

            return GetResponse<ResponseList<MoodTypeReadDTO>, bool>(isAvailable, HttpVerbsEnum.GET, $"/isavailable/{isAvailable}");
        }

        public Task<ResponseViewModel<ResponsePagedList<MoodTypeReadDTO>>> GetPagedAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<MoodTypeReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<ResponseList<MoodTypeReadDTO>>> GetTopTen()
        {
            return GetResponse<ResponseList<MoodTypeReadDTO>, int>(10, HttpVerbsEnum.GET, "/top/10");
        }
    }
}
