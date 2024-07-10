using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostBLL : CRUDBaseBLL<Post, PostCreateDTO, PostReadDTO, PostUpdateDTO>, IPostBLL
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork.Posts, mapper, logger, dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ResponsePagedList<PostPendingToPublishDTO>> GetAllPostsNotPublishedAsync(PagerParams pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllPostsNotPublished(_unitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<ResponsePagedList<PostListDTO>> GetPostsPublishedPagedAsync(PagerParams pagerDTO)
        {
            return await ExecutePagedListAsync(new GetAllPublishedPostsPaged(_unitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task<ResponsePagedList<PostListDTO>> GetPostsPublishedByUserPagedAsync(int UserID, PagerParams pagerDTO)
        {
            return await ExecutePagedListAsync(new GetPostsPublishedByUserPaged(UserID, _unitOfWork, Mapper, pagerDTO), pagerDTO);
        }

        public async Task ApprovePostPublishAsync(int postId)
        {
            var entity = await _unitOfWork.Posts.GetByIdAsync(postId);
            entity.PublishDate = DateTime.Now;
            entity.IsPublished = true;

            await _unitOfWork.Posts.UpdateAsync(entity);
        }
        public async Task<ValidatorResponse> ValidateApprovePostPublishAsync(int postId)
        {

            ResetValidations();

            bool exists = await _unitOfWork.Posts.Query().Where(t => t.Id == postId).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
            else
            {
                var entity = await _unitOfWork.Posts.GetByIdAsync(postId);
                if (entity.IsPublished)
                {
                    _validate.AddError(Helpers.ValidationPostErrorMessages.OnTryPublishAlreadyPublished);
                }
            }

            if (!_validate.IsValid)
                this._logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);


            return this._validate;
        }



        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await _unitOfWork.Posts.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(PostCreateDTO createDTO)
        {
            bool exists = await _unitOfWork.Posts.Query().Where(t => t.Text == createDTO.Text).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, PostUpdateDTO updateDTO)
        {
            bool exists = await _unitOfWork.Posts.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }


    }
}
