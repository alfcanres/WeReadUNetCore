using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Post
{
    public interface IPostRepository : IBaseRepository
    {
        Task<ResponseViewModel<PostReadDTO>> CreateAsync(PostCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostReadDTO>> GetByIdAsync(int id);
        Task<ResponseViewModel<PostReadDTO>> UpdateAsync(PostUpdateDTO updateModel);

        Task<ResponseViewModel<bool>> ApproveAsync(int id);
        Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPublishedPagedByUserAsync(int id, PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPublishedPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPendingPublishPagedAsync(PagerParams pagerDTO);

        Task<ResponseViewModel<ResponsePagedList<PostCommentReadDTO>>> GetPostCommentsPageddAsync(int postId, PagerParams pagerDTO);

        Task<ResponseViewModel<PostVoteViewDTO>> GetPostVotesAsync(int postId);

    }
}
