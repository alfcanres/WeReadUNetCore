using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class MoodTypeBLL : CRUDBaseBLL<MoodType, MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>, IBLL<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>, IMoodTypeBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        public MoodTypeBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MoodTypeBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork.MoodTypes, mapper, logger, dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountAllAsync()
        {
            return await _unitOfWork.PostTypes.Query().CountAsync();
        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetAllAsync()
        {
            return await ExecuteListAsync(new GetAllMoodTypes(_unitOfWork, Mapper));
        }

        public async Task<IResponsePagedListDTO<MoodTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllMoodTypesPaged(_unitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetMoodTypesTopWithPosts(_unitOfWork, Mapper, top));

        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await ExecuteListAsync(new GetAllMoodTypeByIsAvailable(_unitOfWork, Mapper, isAvailable));
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await _unitOfWork.MoodTypes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(MoodTypeCreateDTO createDTO)
        {
            bool exists = await _unitOfWork.MoodTypes.Query().Where(t => t.Mood == createDTO.Mood).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, MoodTypeUpdateDTO updateDTO)
        {
            bool exists = await _unitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }


    }
}
