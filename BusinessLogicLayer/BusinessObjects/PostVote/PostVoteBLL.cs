using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Response;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.DTO.Post;
using DataTransferObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.BusinessObjects
{
    public class PostVoteBLL : BaseBLL<PostVoteCreateDTO, PostVoteResultDTO, PostVoteUpdateDTO>, IPostVoteBLL
    {
        public PostVoteBLL(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostVoteBLL> logger, IDataAnnotationsValidator dataAnnotationsValidator) : base(unitOfWork, mapper, logger, dataAnnotationsValidator)
        {
        }

        public async Task<IResponseDTO<PostVoteViewDTO>> GetVotesByPostIdAsync(int PostId)
        {
            ResetValidations();

            try
            {
                var likes = await UnitOfWork.PostVotes.Query().Where(t => t.PostId == PostId && t.ILikedThis).CountAsync();
                var dislikes = await UnitOfWork.PostVotes.Query().Where(t => t.PostId == PostId && !t.ILikedThis).CountAsync();

                PostVoteViewDTO postview = new PostVoteViewDTO()
                {
                    PostId = PostId,
                    Dislike = dislikes,
                    Like = likes
                };


                ResponseDTO<PostVoteViewDTO> response = new ResponseDTO<PostVoteViewDTO>(postview);

                return response;
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validate.IsValid = false;
                _validate.AddError(friendlyError);
                _logger.LogError(ex, "EXECUTE LIST ERROR GetVotesByPostIdAsync");
                return new ResponseDTO<PostVoteViewDTO>(null);
            }

        }

        protected override async Task ExecuteDeleteAsync(int id)
        {
            await UnitOfWork.PostVotes.DeleteAsync(id);
        }

        protected override Task<PostVoteResultDTO> ExecuteGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        protected override async Task<PostVoteResultDTO> ExecuteInsertAsync(PostVoteCreateDTO createDTO)
        {

            PostVote entity = new PostVote()
            {
                ApplicationUserInfoId = createDTO.ApplicationUserInfoId,
                PostId = createDTO.PostId,
                ILikedThis = true,
                VoteDate = DateTime.Now,
            };

            await UnitOfWork.PostVotes.InsertAsync(entity);


            return new PostVoteResultDTO()
            {
                Id = entity.Id,
                PostTitle = entity.Post.Title
            };
        }

        protected override async Task<PostVoteResultDTO> ExecuteUpdateAsync(PostVoteUpdateDTO updateDTO)
        {
            var entity = await UnitOfWork.PostVotes.GetByIdAsync(updateDTO.Id);

            entity.ILikedThis = !entity.ILikedThis;

            await UnitOfWork.PostVotes.UpdateAsync(entity);

            return new PostVoteResultDTO()
            {
                Id = entity.Id,
                PostTitle = entity.Post.Title
            };
        }

        protected override async Task ExecValidateDeleteAsync(int id)
        {
            bool exists = await UnitOfWork.PostVotes.Query().Where(t => t.Id == id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationErrorMessages.OnDeleteNoRecordWasFound);
            }
        }

        protected override async Task ExecValidateInsertAsync(PostVoteCreateDTO createDTO)
        {
            bool exists = await UnitOfWork.PostVotes.Query().
                Where(
                t =>
                t.ApplicationUserInfoId == createDTO.ApplicationUserInfoId
                &&
                t.PostId == createDTO.PostId
                ).AnyAsync();

            if (exists)
            {
                _validate.AddError(Helpers.ValidationPostErrorMessages.AlreadyVotedForThisPost);
            }
        }

        protected override async Task ExecValidateUpdateAsync(int id, PostVoteUpdateDTO updateDTO)
        {
            bool exists = await UnitOfWork.PostVotes.Query().Where(t => t.Id == updateDTO.Id).AnyAsync();
            if (!exists)
            {
                _validate.AddError(Helpers.ValidationPostErrorMessages.ErrorChangingVoteForThisPost);
            }
        }
    }
}
