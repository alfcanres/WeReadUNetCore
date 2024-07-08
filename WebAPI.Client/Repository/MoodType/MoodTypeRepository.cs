using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
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

        public Task<ResponseViewModel<IResponseListDTO<MoodTypeReadDTO>>> GetIsAvailableAsync(bool isAvailable)
        {

            return GetResponse<IResponseListDTO<MoodTypeReadDTO>, bool>(isAvailable, HttpVerbsEnum.GET, $"/isavailable/{isAvailable}");
        }

        public Task<ResponseViewModel<IResponsePagedListDTO<MoodTypeReadDTO>>> GetPagedAsync(PagerDTO pagerDTO)
        {
            return GetResponse<IResponsePagedListDTO<MoodTypeReadDTO>, PagerDTO>(pagerDTO, HttpVerbsEnum.GET, $"/paged{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<IResponseListDTO<MoodTypeReadDTO>>> GetTopTen()
        {
            return GetResponse<IResponseListDTO<MoodTypeReadDTO>, int>(10, HttpVerbsEnum.GET, "/top/10");
        }
    }
}
