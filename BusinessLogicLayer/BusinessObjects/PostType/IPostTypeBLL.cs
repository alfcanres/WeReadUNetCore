using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostTypeBLL : IBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        Task<IPagedListDTO<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO);
        Task<int> CountAllAsync();
        Task<IListDTO<PostTypeReadDTO>> GetAllAsync();
        Task<IListDTO<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<IListDTO<PostTypeReadDTO>> GetTopWithPostsAsync(int top);


    }
}