using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostTypeBLL : BaseBLL<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>, IPostTypeBLL
    {
        public PostTypeBLL(IUnitOfWork unitOfWork, IMapper mapper, IValidate validate) : base(unitOfWork, mapper, validate)
        {

        }


        public Task<int> CountAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PostTypeReadDTO>> GetAll()
        {
            return await ExecuteListAsync(new GetAllPostTypes(UnitOfWork, Mapper));
        }

        public Task<IEnumerable<PostTypeReadDTO>> GetAllByIsAvailable(bool isAvailable)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostTypeReadDTO>> GetAllPaged(IPagerDTO pagerDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostTypeReadDTO>> GetTopWithPosts(int top)
        {
            throw new NotImplementedException();
        }

        public override async Task<PostTypeReadDTO> GetByIdAsync(int id)
        {
            var entity = await UnitOfWork.PostTypes.GetByIdAsync(id);
            var dto = Mapper.Map<PostTypeReadDTO>(entity);
            return dto;
        }


        #region CREATE, UPDATE, DELETE BASE METHODS

        protected override async Task<bool> ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.PostVotes.DeleteAsync(id);
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
            if (!exists)
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
