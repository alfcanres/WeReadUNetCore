using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostVoteBLL 
    {
        Task<IValidate> ValidateDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<IValidate> ValidateInsertAsync(PostVoteCreateDTO createDTO);
        Task<IResponseDTO<PostVoteResultDTO>> InsertAsync(PostVoteCreateDTO createDTO);
        Task<IValidate> ValidateUpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<IResponseDTO<PostVoteResultDTO>> UpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<IResponseDTO<PostVoteViewDTO>> GetVotesByPostIdAsync(int PostId);
    }
}
