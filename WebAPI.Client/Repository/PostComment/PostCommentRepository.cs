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
using WebAPI.Client.Repository.PostComment;
using WebAPI.Client.ViewModels;

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
