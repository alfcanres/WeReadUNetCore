using DataTransferObjects.DTO;
using WebAPI.Client.Helpers;

namespace WebAPI.Client.Repository.PostComment
{
    public class PostCommentRepository :
        BaseRepository<PostCommentCreateDTO, PostCommentReadDTO, PostCommentUpdateDTO>,
        IPostCommentRepository
    {
        public PostCommentRepository(IHttpClientHelper httpClientHelper) 
            :base("api/postcomment",httpClientHelper)
        {

        }

    }
}
