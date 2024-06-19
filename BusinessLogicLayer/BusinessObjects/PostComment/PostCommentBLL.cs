using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostCommentBLL : CRUDBaseBLL<PostComment, PostCommentCreateDTO, PostCommentReadDTO, PostCommentUpdateDTO>, IPostCommentBLL
    {
        IUnitOfWork _unitOfWork;
        public PostCommentBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostCommentBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork.PostComments, mapper, logger, dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IResponsePagedListDTO<PostCommentReadDTO>> GetPagedByPostIdAsync(int postID, IPagerDTO pagerDTO)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecValidateInsertAsync(PostCommentCreateDTO createDTO)
        {
            bool exists = await _unitOfWork.PostComments.Query().
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

        protected override async Task ExecValidateUpdateAsync(int id, PostCommentUpdateDTO updateDTO)
        {
            bool exists = await _unitOfWork.PostComments.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await _unitOfWork.PostComments.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }
    }
}
