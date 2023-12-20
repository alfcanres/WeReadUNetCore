using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostTypeBLL : IBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        Task<IEnumerable<PostTypeReadDTO>> GetAllPaged(IPagerDTO pagerDTO);
        Task<int> CountAll();
        Task<IEnumerable<PostTypeReadDTO>> GetAll();
        Task<IEnumerable<PostTypeReadDTO>> GetAllByIsAvailable(bool isAvailable);
        Task<IEnumerable<PostTypeReadDTO>> GetTopWithPosts(int top);


    }
}