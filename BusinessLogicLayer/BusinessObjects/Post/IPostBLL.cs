using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostBLL : IBLL<PostCreateDTO, PostReadDTO, PostUpdateDTO>
    {

        Task<IValidate> ValidateApprovePostPublishAsync(int postId);
        Task ApprovePostPublishAsync(int postId);
        Task<IResponsePagedListDTO<PostPendingToPublishDTO>> GetAllPostsNotPublishedAsync(IPagerDTO pagerDTO);
        Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedPagedAsync(IPagerDTO pagerDTO);
        Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedByUserPagedAsync(int UserID, IPagerDTO pagerDTO);


    }
}