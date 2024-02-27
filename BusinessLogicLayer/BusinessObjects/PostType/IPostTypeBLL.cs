using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostTypeBLL : IBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        Task<IResponsePagedListDTO<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO);
        Task<int> CountAllAsync();
        Task<IResponseListDTO<PostTypeReadDTO>> GetAllAsync();
        Task<IResponseListDTO<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<IResponseListDTO<PostTypeReadDTO>> GetTopWithPostsAsync(int top);


    }
}