using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
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

        public async Task<IListDTO<PostTypeReadDTO>> GetAllAsync()
        {
            IEnumerable<PostTypeReadDTO> list = await ExecuteListAsync(new GetAllPostTypes(UnitOfWork, Mapper));

            return new ListDTO<PostTypeReadDTO>(list, this._validate);
        }



        public async Task<IPagedListDTO<PostTypeReadDTO>> GetAllPagedAsync(IPagerDTO pagerDTO)
        {
            int recordCount = await CountAllAsync();

            IEnumerable<PostTypeReadDTO> list = await ExecuteListAsync(new GetAllPostTypePaged(UnitOfWork, Mapper, pagerDTO));

            return new PagedListDTO<PostTypeReadDTO>(list, recordCount, pagerDTO, this._validate);
        }

        public async Task<IListDTO<PostTypeReadDTO>> GetTopWithPostsAsync(int top)
        {
            IEnumerable<PostTypeReadDTO> list = await ExecuteListAsync(new GetPostTypeTopWithPosts(UnitOfWork, Mapper, top));

            return new ListDTO<PostTypeReadDTO>(list, this._validate);
        }

        public async Task<IListDTO<PostTypeReadDTO>> GetAllByIsAvailableAsync(bool isAvailable)
        {
            IEnumerable<PostTypeReadDTO> list = await ExecuteListAsync(new GetAllPostTypeByIsAvailable(UnitOfWork, Mapper, isAvailable));

            return new ListDTO<PostTypeReadDTO>(list, this._validate);
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
                _validate.AddError("No record was found or was previously deleted");
            }
        }

        protected override async Task ExecValidateInsertAsync(PostTypeCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Description == createDTO.Description).AnyAsync();
            if (exists)
            {
                _validate.AddError("An item with the same description already exists, please type another");
            }
        }

        protected override async Task ExecValidateUpdateAsync(PostTypeUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostTypes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError("No record was found or was previously deleted");
            }
        }





        #endregion
    }
}
