using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostCommentBLL : BaseBLL<PostCommentCreateDTO, PostCommentReadDTO, PostCommentUpdateDTO>, IPostCommentBLL
    {
        public PostCommentBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }
        public Task<IResponsePagedListDTO<PostCommentReadDTO>> GetPagedByPostId(int postID, IPagerDTO pagerDTO)
        {
            throw new NotImplementedException();
        }

        protected override async Task<PostCommentReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.PostComments.GetByIdAsync(id);
            var dto = Mapper.Map<PostCommentReadDTO>(entity);
            return dto;
        }

        protected override async Task<PostCommentReadDTO> ExecuteInsertAsync(PostCommentCreateDTO createDTO)
        {
            PostComment entity = Mapper.Map<PostComment>(createDTO);
            await UnitOfWork.PostComments.InsertAsync(entity);
            var dto = Mapper.Map<PostCommentReadDTO>(entity);
            return dto;
        }

        protected override async Task<PostCommentReadDTO> ExecuteUpdateAsync(PostCommentUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.PostComments.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.PostComments.UpdateAsync(entity);
            var dto = Mapper.Map<PostCommentReadDTO>(entity);
            return dto;
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            await UnitOfWork.PostComments.DeleteAsync(id);
        }

        protected override async Task ExecValidateInsertAsync(PostCommentCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.PostComments.Query().
                Where(
                t => 
                t.CommentText == createDTO.CommentText
                &&
                t.ApplicationUserInfoId == createDTO.ApplicationUserInfoId
                &&
                t.PostId == createDTO.PostId                
                ).AnyAsync();

            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(PostCommentUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostComments.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

        protected override async Task ExecuteDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.PostComments.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }


    }
}
