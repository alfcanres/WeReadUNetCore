using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.MoodType
{
    public interface IMoodTypeRepository : IBaseRepository
    {
        Task<ResponseViewModel<IResponseDTO<MoodTypeReadDTO>>> CreateAsync(MoodTypeCreateDTO createModel);
        Task<IValidate> DeleteAsync(int id);
        Task<ResponseViewModel<IResponseDTO<MoodTypeReadDTO>>> GetByIdAsync(int id);
        Task<ResponseViewModel<IResponseDTO<MoodTypeReadDTO>>> UpdateAsync(MoodTypeUpdateDTO updateModel);

        Task<ResponseViewModel<IResponsePagedListDTO<MoodTypeReadDTO>>> GetPagedAsync(PagerDTO pagerDTO);

        Task<ResponseViewModel<IResponseListDTO<MoodTypeReadDTO>>> GetIsAvailableAsync(bool isAvailable);

        Task<ResponseViewModel<IResponseListDTO<MoodTypeReadDTO>>> GetTopTen();

    }
}
