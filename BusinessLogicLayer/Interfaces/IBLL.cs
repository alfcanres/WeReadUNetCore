using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBLL<CreateDTO, ReadDTO, UpdateDTO> 
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        Task<IValidate> DeleteAsync(int id);
        Task<IResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO);
        Task<IResponseDTO<ReadDTO>> UpdateAsync(UpdateDTO updateDTO);
        Task<IResponseDTO<ReadDTO>> GetByIdAsync(int id);
    }
}