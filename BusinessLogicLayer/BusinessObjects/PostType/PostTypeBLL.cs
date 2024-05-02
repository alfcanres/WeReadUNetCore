using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer.BusinessObjects
{
    public class PostTypeBLL : BaseBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>, IPostTypeBLL
    {
        public PostTypeBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostTypeBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }

        public async Task<int> CountAllAsync()
        {
            return await UnitOfWork.PostTypes.Query().CountAsync();
        }

        public async Task<IResponseListDTO<PostTypeReadDTO>> GetAllAsync()
        {
            return await ExecuteListAsync(new GetAllPostTypes(UnitOfWork, Mapper));
        }

        public async Task<IResponsePagedListDTO<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllPostTypePaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<IResponseListDTO<PostTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetPostTypeTopWithPosts(UnitOfWork, Mapper, top));
        }

        public async Task<IResponseListDTO<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await ExecuteListAsync(new GetAllPostTypeByIsAvailable(UnitOfWork, Mapper, isAvailable));
        }

        protected override async Task<PostTypeReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.PostTypes.GetByIdAsync(id);
            var dto = Mapper.Map<PostTypeReadDTO>(entity);
            return dto;
        }

        #region CREATE, UPDATE, DELETE BASE METHODS

        protected override async Task ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.PostTypes.DeleteAsync(id);
        }

        protected override async Task<PostTypeReadDTO> ExecuteInsertAsync(PostTypeCreateDTO createDTO)
        {
            PostType postType = Mapper.Map<PostType>(createDTO);
            await UnitOfWork.PostTypes.InsertAsync(postType);
            var dto = Mapper.Map<PostTypeReadDTO>(postType);
            return dto;
        }

        protected override async Task<PostTypeReadDTO> ExecuteUpdateAsync(PostTypeUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.PostTypes.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.PostTypes.UpdateAsync(entity);
            var dto = Mapper.Map<PostTypeReadDTO>(entity);
            return dto;
        }


        #endregion

        #region Validations

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(PostTypeCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Description == createDTO.Description).AnyAsync();
            
               if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(PostTypeUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }





        #endregion
    }
}
