using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class MoodTypeBLL : BaseBLL<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>, IBLL<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>, IMoodTypeBLL
    {
        public MoodTypeBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }

        public async Task<int> CountAllAsync()
        {
            return await UnitOfWork.PostTypes.Query().CountAsync();
        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetAllAsync()
        {
            return await ExecuteListAsync(new GetAllMoodTypes(UnitOfWork, Mapper));
        }

        public async Task<IResponsePagedListDTO<MoodTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllMoodTypesPaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetMoodTypesTopWithPosts(UnitOfWork, Mapper, top));

        }

        public async Task<IResponseListDTO<MoodTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await ExecuteListAsync(new GetAllMoodTypeByIsAvailable(UnitOfWork, Mapper, isAvailable));
        }

        protected override async Task<MoodTypeReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.MoodTypes.GetByIdAsync(id);
            var dto = Mapper.Map<MoodTypeReadDTO>(entity);
            return dto;
        }

        protected override async Task<MoodTypeReadDTO> ExecuteInsertAsync(MoodTypeCreateDTO createDTO)
        {
            MoodType entity = Mapper.Map<MoodType>(createDTO);
            await UnitOfWork.MoodTypes.InsertAsync(entity);
            var dto = Mapper.Map<MoodTypeReadDTO>(entity);
            return dto;
        }

        protected override async Task<MoodTypeReadDTO> ExecuteUpdateAsync(MoodTypeUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.MoodTypes.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.MoodTypes.UpdateAsync(entity);
            var dto = Mapper.Map<MoodTypeReadDTO>(entity);
            return dto;
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            await UnitOfWork.MoodTypes.DeleteAsync(id);
        }

        protected override async Task ExecValidateInsertAsync(MoodTypeCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.MoodTypes.Query().Where(t => t.Mood == createDTO.Mood).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(MoodTypeUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

        protected override async Task ExecuteDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.MoodTypes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }
    }
}
