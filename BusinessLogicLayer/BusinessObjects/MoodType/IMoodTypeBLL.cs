using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IMoodTypeBLL
    {
        Task<int> CountAllAsync();
        Task<IResponseListDTO<MoodTypeReadDTO>> GetAllAsync();
        Task<IResponseListDTO<MoodTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<IResponsePagedListDTO<MoodTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO);
        Task<IResponseListDTO<MoodTypeReadDTO>> GetTopWithPostsAsync(int top);
    }
}