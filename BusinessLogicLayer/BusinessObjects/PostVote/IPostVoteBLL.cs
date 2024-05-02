using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer.BusinessObjects
{
    public interface IPostVoteBLL 
    {
        Task<IValidate> DeleteAsync(int id);
        Task<IResponseDTO<PostVoteResultDTO>> InsertAsync(PostVoteCreateDTO createDTO);
        Task<IResponseDTO<PostVoteResultDTO>> UpdateAsync(PostVoteUpdateDTO updateDTO);
        Task<IResponseDTO<PostVoteViewDTO>> GetVotesByPostIdAsync(int PostId);
    }
}
