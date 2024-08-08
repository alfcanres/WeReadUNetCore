using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.Helpers;
using WebAPI.Client.Repository.MoodType;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.Post
{
    public class PostRepository :
        BaseRepository<PostCreateDTO, PostReadDTO, PostUpdateDTO>,
        IPostRepository
    {

        public PostRepository(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BaseRepository<PostCreateDTO, PostReadDTO, PostUpdateDTO>> logger
            ) :
            base(
            "api/post",
            httpClientFactory,
            configuration,
            logger
            )
        {

        }

        public Task<ResponseViewModel<bool>> ApproveAsync(int id)
        {
            return GetValidateResponse<int>(id, HttpVerbsEnum.POST, $"/approve");
        }


        public Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPendingPublishPagedAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<PostReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/pendingpublishpaged{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<ResponsePagedList<PostCommentReadDTO>>> GetPostCommentsPageddAsync(int postId, PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<PostCommentReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/{postId}/comments{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<PostVoteViewDTO>> GetPostVotesAsync(int postId)
        {
            return GetResponse<PostVoteViewDTO, int>(postId, HttpVerbsEnum.GET, $"/{postId}/votes");
        }

        public Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPublishedPagedAsync(PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<PostReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/publishedpaged{pagerDTO.ToQueryString()}");
        }

        public Task<ResponseViewModel<ResponsePagedList<PostReadDTO>>> GetPublishedPagedByUserAsync(int id, PagerParams pagerDTO)
        {
            return GetResponse<ResponsePagedList<PostReadDTO>, PagerParams>(pagerDTO, HttpVerbsEnum.GET, $"/publishedpagedbyuser{pagerDTO.ToQueryString()}&id{id}");
        }
    }
}
