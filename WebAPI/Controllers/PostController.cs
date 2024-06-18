using BusinessLogicLayer.BusinessObjects;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : BaseController<PostCreateDTO, PostReadDTO, PostUpdateDTO>
    {
        private readonly IPostBLL _BLL;
        private readonly IPostCommentBLL _postCommentBLL;
        private readonly IPostVoteBLL _postVoteBLL;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostBLL BLL, IPostCommentBLL postCommentBLL, IPostVoteBLL postVoteBLL, ILogger<PostController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
            _postCommentBLL = postCommentBLL;
            _postVoteBLL = postVoteBLL;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
        }


        [HttpPut("Approve")]
        public async Task<ActionResult> Put(int id)
        {

            var response = await _BLL.ApprovePostPublish(id);

            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return await GetByIdAsync(id);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 10)]
        [HttpGet("PublishedPaged")]
        public async Task<ActionResult> GetPublishedPaged([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetPostsPublishedPaged(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [ResponseCache(Duration = 10)]
        [HttpGet("PublishedPagedByUser/{id}")]
        public async Task<ActionResult> GetPaged(int id, [FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetPostsPublishedByUserPaged(id, pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("PendingPublishPaged")]
        public async Task<ActionResult> GetNotPublished([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetAllPostsNotPublished(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("{id}/Comments")]
        public async Task<ActionResult> GetCommentsPaged(int id, [FromQuery] PagerDTO pagerDTO)
        {
            var result = await _postCommentBLL.GetPagedByPostId(id, pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [HttpGet("{id}/Votes")]
        public async Task<ActionResult> GetVotes(int id)
        {
            var result = await _postVoteBLL.GetVotesByPostIdAsync(id);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }

    }
}
