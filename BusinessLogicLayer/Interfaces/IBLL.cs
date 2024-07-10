using DataTransferObjects;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBLL<CreateDTO, ReadDTO, UpdateDTO>
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        Task DeleteAsync(int id);
        Task<Response<ReadDTO>> InsertAsync(CreateDTO createDTO);
        Task<Response<ReadDTO>> UpdateAsync(int id, UpdateDTO updateDTO);
        Task<Response<ReadDTO>> GetByIdAsync(int id);
        Task<ValidatorResponse> ValidateInsertAsync(CreateDTO createDTO);
        Task<ValidatorResponse> ValidateDeleteAsync(int id);
        Task<ValidatorResponse> ValidateUpdateAsync(int id, UpdateDTO updateDTO);
    }
}