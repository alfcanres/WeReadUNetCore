using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBLL<CreateDTO, ReadDTO, UpdateDTO>
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        Task DeleteAsync(int id);
        Task<IResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO);
        Task<IResponseDTO<ReadDTO>> UpdateAsync(int id, UpdateDTO updateDTO);
        Task<IResponseDTO<ReadDTO>> GetByIdAsync(int id);
        Task<IValidate> ValidateInsertAsync(CreateDTO createDTO);
        Task<IValidate> ValidateDeleteAsync(int id);
        Task<IValidate> ValidateUpdateAsync(int id, UpdateDTO updateDTO);
    }
}