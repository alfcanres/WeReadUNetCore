using BusinessLogicLayer.BusinessObjects;
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
    public class PostCommentController : Controller
    {
        private readonly IPostCommentBLL _BLL;
        private readonly ILogger<PostCommentController> _logger;

        public PostCommentController(IPostCommentBLL BLL, ILogger<PostCommentController> logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostCommentCreateDTO createModel)
        {
            var response = await _BLL.InsertAsync(createModel);
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
        public async Task<ActionResult<PostCommentReadDTO>> Get(int id)
        {
            var response = await _BLL.GetByIdAsync(id);
            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            IValidate response = await _BLL.DeleteAsync(id);
            if (response.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


    }
}
