using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBLL<CreateDTO, ReadDTO, UpdateDTO>
    {
        Task<IValidate> ValidateDeleteAsync(int id);
        Task<bool> DeleteAsync(int id);

        Task<IValidate> ValidateInsertAsync(CreateDTO createDTO);
        Task<ReadDTO> InsertAsync(CreateDTO createDTO);

        Task<IValidate> ValidateUpdateAsync(UpdateDTO updateDTO);
        Task<ReadDTO> UpdateAsync(UpdateDTO updateDTO);

        Task<ReadDTO> GetByIdAsync(int id);

        IValidate IsOperationValid();


    }
}