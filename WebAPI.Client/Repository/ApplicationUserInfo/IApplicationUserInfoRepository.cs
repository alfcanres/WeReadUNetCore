using DataTransferObjects.DTO;
using DataTransferObjects;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.ApplicationUserInfo
{
    public interface IApplicationUserInfoRepository : IBaseRepository
    {
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> CreateAsync(ApplicationUserInfoCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<ApplicationUserInfoReadDTO>> UpdateAsync(ApplicationUserInfoUpdateDTO updateModel);

        Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoReadDTO>>> GetPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<ApplicationUserInfoReadDTO>>> GetPagedByActiveAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponseList<ApplicationUserInfoReadDTO>>> GetTopTenAsync();
    }
}
