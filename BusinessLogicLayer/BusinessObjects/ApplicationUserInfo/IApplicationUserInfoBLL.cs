using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer.BusinessObjects
{
    public interface IApplicationUserInfoBLL : IBLL<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>
    {
        Task<IResponsePagedListDTO<ApplicationUserInfoListDTO>> GetAllActivePagedAsync(IPagerDTO pagerDTO);
        Task<IResponseListDTO<ApplicationUserInfoListDTO>> GetTopWithPostsAsync(int top);
        Task<IResponsePagedListDTO<ApplicationUserInfoListDTO>> GetAllPagedAsync(IPagerDTO pagerDTO);

    }
}
