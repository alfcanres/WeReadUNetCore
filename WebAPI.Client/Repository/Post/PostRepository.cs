using DataTransferObjects;
using DataTransferObjects.DTO;
using WebAPI.Client.Helpers;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Post
{
    public class PostRepository :
        BaseRepository<PostCreateDTO, PostReadDTO, PostUpdateDTO>,
        IPostRepository
    {

        public PostRepository(IHttpClientHelper httpClientHelper) :
            base("api/post", httpClientHelper)
        {

        }

        public async Task<ResponseViewModel<bool>> ApproveAsync(int id)
        {
            return await HttpClientHelper.GetValidateResponse<int>(id, HttpVerbsEnum.PUT, $"{BaseEndPoint}/approve/{id}");
        }


        public async Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPendingPublishPagedAsync(PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint} /pendingpublishpaged{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<ResponsePagedList<PostCommentReadDTO>>> GetPostCommentsPageddAsync(int postId, PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostCommentReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/{postId}/comments{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<PostVoteViewDTO>> GetPostVotesAsync(int postId)
        {
            return await HttpClientHelper.GetResponse<PostVoteViewDTO, int>(postId, HttpVerbsEnum.GET, $"{BaseEndPoint}/{postId}/votes");
        }

        public async Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPublishedPagedAsync(PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/publishedpaged{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetPublishedPagedByUserAsync(int id, PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/publishedpagedbyuser/{id}{pagerDTO.ToQueryString()}");
        }

        public async Task<ResponseViewModel<ResponsePagedList<PostListDTO>>> GetAllPagedByUserAsync(int id, PagerParams pagerDTO)
        {
            return await HttpClientHelper.GetResponse<ResponsePagedList<PostListDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"{BaseEndPoint}/pagebyuser/{id}{pagerDTO.ToQueryString()}");
        }
    }
}
