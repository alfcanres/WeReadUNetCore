using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostBLL : IBLL<PostCreateDTO, PostReadDTO, PostUpdateDTO>
    {
        Task<IResponseDTO<PostReadDTO>> ApprovePostPublish(int postId);
        Task<IResponsePagedListDTO<PostPendingToPublishDTO>> GetAllPostsNotPublished(IPagerDTO pagerDTO);
        Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedPaged(IPagerDTO pagerDTO);
        Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedByUserPaged(int UserID, IPagerDTO pagerDTO);


    }
}