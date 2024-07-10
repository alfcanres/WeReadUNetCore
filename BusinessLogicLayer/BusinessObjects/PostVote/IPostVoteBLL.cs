using DataTransferObjects;
using DataTransferObjects.DTO;



namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostVoteBLL 
    {
        Task<ValidatorResponse> ValidateDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<ValidatorResponse> ValidateInsertAsync(PostVoteCreateDTO createDTO);
        Task<Response<PostVoteResultDTO>> InsertAsync(PostVoteCreateDTO createDTO);
        Task<ValidatorResponse> ValidateUpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<Response<PostVoteResultDTO>> UpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<Response<PostVoteViewDTO>> GetVotesByPostIdAsync(int PostId);
    }
}
