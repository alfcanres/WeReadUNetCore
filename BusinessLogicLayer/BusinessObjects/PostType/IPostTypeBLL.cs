using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostTypeBLL : IBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        Task<IEnumerable<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO);
        Task<int> CountAllAsync();
        Task<IEnumerable<PostTypeReadDTO>> GetAllAsync();
        Task<IEnumerable<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable);
        Task<IEnumerable<PostTypeReadDTO>> GetTopWithPostsAsync(int top);


    }
}