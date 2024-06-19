using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostCommentBLL 
    {
        Task<IValidate> ValidateDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<IResponseDTO<PostCommentReadDTO>> InsertAsync(PostCommentCreateDTO createDTO);
        Task<IValidate> ValidateInsertAsync(PostCommentCreateDTO createDTO);        
        Task<IResponseDTO<PostCommentReadDTO>> GetByIdAsync(int id);
        Task<IResponsePagedListDTO<PostCommentReadDTO>> GetPagedByPostIdAsync(int postID, IPagerDTO pagerDTO);



    }
}
