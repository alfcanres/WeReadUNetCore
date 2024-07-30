using DataTransferObjects;
using DataTransferObjects.DTO;



namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostVoteBLL 
    {
        Task<ValidatorResponse> ValidateDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<ValidatorResponse> ValidateInsertAsync(PostVoteCreateDTO createDTO);
        Task<PostVoteResultDTO> InsertAsync(PostVoteCreateDTO createDTO);
        Task<ValidatorResponse> ValidateUpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<PostVoteResultDTO> UpdateAsync(int id, PostVoteUpdateDTO updateDTO);
        Task<PostVoteViewDTO> GetVotesByPostIdAsync(int PostId);
    }
}
