using DataTransferObjects;
using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBLL<CreateDTO, ReadDTO, UpdateDTO> 
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        Task<IValidate> DeleteAsync(int id);
        Task<ResultResponseDTO<ReadDTO>> InsertAsync(CreateDTO createDTO);
        Task<ResultResponseDTO<ReadDTO>> UpdateAsync(UpdateDTO updateDTO);
        Task<ResultResponseDTO<ReadDTO>> GetByIdAsync(int id);
    }
}