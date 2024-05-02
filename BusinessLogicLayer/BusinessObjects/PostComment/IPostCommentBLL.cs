using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostCommentBLL 
    {
        Task<IValidate> DeleteAsync(int id);
        Task<IResponseDTO<PostCommentReadDTO>> InsertAsync(PostCommentCreateDTO createDTO);
        Task<IResponseDTO<PostCommentReadDTO>> GetByIdAsync(int id);
        Task<IResponsePagedListDTO<PostCommentReadDTO>> GetPagedByPostId(int postID, IPagerDTO pagerDTO);

    }
}
