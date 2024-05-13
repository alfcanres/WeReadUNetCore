using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostBLL : BaseBLL<PostCreateDTO, PostReadDTO, PostUpdateDTO>, IPostBLL
    {
        public PostBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }

        protected override async Task<PostReadDTO> ExecuteGetByIdAsync(int id)
        {
            var entity = await UnitOfWork.Posts.GetByIdAsync(id);
            var dto = Mapper.Map<PostReadDTO>(entity);
            return dto;
        }

        public async Task<IResponsePagedListDTO<PostPendingToPublishDTO>> GetAllPostsNotPublished(IPagerDTO pagerDTO)
        {

            return await ExecutePagedListAsync(new GetAllPostsNotPublished(UnitOfWork, Mapper, pagerDTO), pagerDTO);


            //ResetValidations();
            //try
            //{
            //    var queryStrategy = new GetAllPostsNotPublished(UnitOfWork, Mapper, pagerDTO);
            //    return new ResponseListDTO<PostPendingToPublishDTO>(await queryStrategy.GetResultsAsync(), this._validate);
            //}
            //catch (Exception ex)
            //{
            //    string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
            //    _validate.IsValid = false;
            //    _validate.AddError(friendlyError);
            //    _logger.LogError(ex, "EXECUTE LIST ERROR GetAllPostsNotPublished");
            //    return new ResponseListDTO<PostPendingToPublishDTO>(this._validate);
            //}

        }

        public async Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedPaged(IPagerDTO pagerDTO)
        {

            return await ExecutePagedListAsync(new GetAllPublishedPostsPaged(UnitOfWork, Mapper, pagerDTO), pagerDTO);
            //ResetValidations();
            //try
            //{
            //    var queryStrategy = new GetAllPublishedPostsPaged(UnitOfWork, Mapper, pagerDTO);
            //    return new ResponseListDTO<PostListDTO>(await queryStrategy.GetResultsAsync(), this._validate);
            //}
            //catch (Exception ex)
            //{
            //    string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
            //    _validate.IsValid = false;
            //    _validate.AddError(friendlyError);
            //    _logger.LogError(ex, "EXECUTE LIST ERROR GetAllPublishedPostsPaged");
            //    return new ResponseListDTO<PostListDTO>(this._validate);
            //}
        }

        public async Task<IResponsePagedListDTO<PostListDTO>> GetPostsPublishedByUserPaged(int UserID, IPagerDTO pagerDTO)
        {

            return await ExecutePagedListAsync(new GetPostsPublishedByUserPaged(UserID, UnitOfWork, Mapper, pagerDTO), pagerDTO);

            //ResetValidations();
            //try
            //{
            //    var queryStrategy = new GetPostsPublishedByUserPaged(UserID, UnitOfWork, Mapper, pagerDTO);
            //    return new ResponseListDTO<PostListDTO>(await queryStrategy.GetResultsAsync(), this._validate);
            //}
            //catch (Exception ex)
            //{
            //    string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
            //    _validate.IsValid = false;
            //    _validate.AddError(friendlyError);
            //    _logger.LogError(ex, "EXECUTE LIST ERROR GetPostsPublishedByUserPaged");
            //    return new ResponseListDTO<PostListDTO>(this._validate);
            //}
        }

        #region CREATE, UPDATE, DELETE BASE METHODS
        protected override async Task ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.Posts.DeleteAsync(id);
        }
        protected override async Task<PostReadDTO> ExecuteInsertAsync(PostCreateDTO createDTO)
        {
            Post entity = Mapper.Map<Post>(createDTO);
            await UnitOfWork.Posts.InsertAsync(entity);
            var dto = Mapper.Map<PostReadDTO>(entity);
            return dto;
        }
        protected override async Task<PostReadDTO> ExecuteUpdateAsync(PostUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.Posts.GetByIdAsync(updateDTO.Id);
            Mapper.Map(updateDTO, entity);
            await UnitOfWork.Posts.UpdateAsync(entity);
            var dto = Mapper.Map<PostReadDTO>(entity);
            return dto;
        }
        public async Task<IResponseDTO<PostReadDTO>> ApprovePostPublish(int postId)
        {

            try
            {
                await ValidateApprovePostPublish(postId);
                if (this._validate.IsValid)
                {
                    var entity = await UnitOfWork.Posts.GetByIdAsync(postId);

                    entity.PublishDate = DateTime.Now;
                    entity.IsPublished = true;

                    await UnitOfWork.Posts.UpdateAsync(entity);
                }
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnUpdateOpeation;
                _validate.IsValid = false;
                _validate.MessageList.Add(friendlyError);
                _logger.LogError(ex, "PUBLISH OPERATION : Post with ID {id}", postId);
            }

            PostReadDTO readDTO = await ExecuteGetByIdAsync(postId);

            return new ResponseDTO<PostReadDTO>(readDTO, this._validate);

        }
        private async Task ValidateApprovePostPublish(int postId)
        {

            ResetValidations();

            bool exists = await UnitOfWork.Posts.Query().Where(t => t.Id == postId).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
            else
            {
                var entity = await UnitOfWork.Posts.GetByIdAsync(postId);
                if (entity.IsPublished)
                {
                    _validate.AddError(Helpers.ValidationPostErrorMessages.OnTryPublishAlreadyPublished);
                }
            }

            if (!_validate.IsValid)
                this._logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);

        }


        #endregion

        #region Validations

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.Posts.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(PostCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.Posts.Query().Where(t => t.Text == createDTO.Text).AnyAsync();
            if (exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnInsertAnItemAlreadyExists);
            }
        }

        protected override async Task ExecValidateUpdateAsync(PostUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.Posts.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnUpdateNoRecordWasFound);
            }
        }

        #endregion
    }
}
