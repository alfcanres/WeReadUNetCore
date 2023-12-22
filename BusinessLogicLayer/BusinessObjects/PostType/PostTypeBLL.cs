using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostTypeBLL : BaseBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>, IPostTypeBLL
    {
        public PostTypeBLL(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }


        public async Task<int> CountAllAsync()
        {
            return await UnitOfWork.PostTypes.Query().CountAsync();
        }

        public async Task<IEnumerable<PostTypeReadDTO>> GetAllAsync()
        {
            return await ExecuteListAsync(new GetAllPostTypes(UnitOfWork, Mapper));
        }

        public async Task<IEnumerable<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            return await ExecuteListAsync(new GetAllPostTypeByIsAvailable(UnitOfWork, Mapper, isAvailable));
        }

        public async Task<IEnumerable<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            return await ExecuteListAsync(new GetAllPostTypePaged(UnitOfWork, Mapper, pagerDTO));
        }

        public async Task<IEnumerable<PostTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            return await ExecuteListAsync(new GetPostTypeTopWithPosts(UnitOfWork, Mapper, top));
        }

        protected override async Task<PostTypeReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.PostTypes.GetByIdAsync(id);
            var dto = Mapper.Map<PostTypeReadDTO>(entity);
            return dto;
        }


        #region CREATE, UPDATE, DELETE BASE METHODS

        protected override async Task<bool> ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.PostTypes.DeleteAsync(id);
            return true;
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

        protected override async Task<IValidate> ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError("No record was found or was previously deleted");
            }
            return _validate;
        }

        protected override async Task<IValidate> ExecValidateInsertAsync(PostTypeCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Description == createDTO.Description).AnyAsync();
            if (exists)
            {
                _validate.AddError("An item with the same description already exists, please type another");
            }
            return _validate;
        }

        protected override async Task<IValidate> ExecValidateUpdateAsync(PostTypeUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError("No record was found or was previously deleted");
            }
            return _validate;
        }

        #endregion 
    }
}
