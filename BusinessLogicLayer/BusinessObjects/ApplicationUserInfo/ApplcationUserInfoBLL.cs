using AutoMapper;
using BusinessLogicLayer.BusinessObject;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class ApplcationUserInfoBLL : BaseBLL<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>, IApplicationUserInfoBLL
    {
        public ApplcationUserInfoBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ApplcationUserInfoBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }

        public async Task<IResponsePagedListDTO<ApplicationUserInfoListDTO>> GetAllActivePagedAsync(IPagerDTO pagerDTO)
        {

            return await ExecutePagedListAsync(new GetAllUsersInfoActivePaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<IResponsePagedListDTO<ApplicationUserInfoListDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllUsersInfoPaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<IResponseListDTO<ApplicationUserInfoListDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetTopUsersInfoWithPosts(UnitOfWork, Mapper, top));
        }

        protected override async Task ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.UsersInfo.DeleteAsync(id);
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.UsersInfo.GetByIdAsync(id);
            var dto = Mapper.Map<ApplicationUserInfoReadDTO>(entity);
            return dto;
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteInsertAsync(ApplicationUserInfoCreateDTO createDTO)
        {
            ApplicationUserInfo entity = Mapper.Map<ApplicationUserInfo>(createDTO);
            await UnitOfWork.UsersInfo.InsertAsync(entity);
            var dto = Mapper.Map<ApplicationUserInfoReadDTO>(entity);
            return dto;
        }

        protected override async Task<ApplicationUserInfoReadDTO> ExecuteUpdateAsync(ApplicationUserInfoUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.UsersInfo.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.UsersInfo.UpdateAsync(entity);
            var dto = Mapper.Map<ApplicationUserInfoReadDTO>(entity);
            return dto;
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(ApplicationUserInfoCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.UserName == createDTO.UserName).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, ApplicationUserInfoUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.UsersInfo.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }

            //TODO apply a business rule for those elements trying to change it's username to an already existent user name
        }
    }
}
