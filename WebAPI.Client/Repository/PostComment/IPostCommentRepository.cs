using DataTransferObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostComment
{
    public interface IPostCommentRepository : IBaseRepository
    {
        Task<ResponseViewModel<PostCommentReadDTO>> CreateAsync(PostCommentCreateDTO createModel);
        Task<ResponseViewModel<bool>> DeleteAsync(int id);
        Task<ResponseViewModel<PostCommentReadDTO>> GetByIdAsync(int id);        
    }
}
