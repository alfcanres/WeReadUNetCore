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
    public class PostTypeController : CustomControllerBase<PostTypeCreateDTO, PostTypeReadDTO, PostTypeUpdateDTO>
    {
        private readonly IPostTypeBLL _BLL;
        private readonly ILogger<PostTypeController> _logger;

        public PostTypeController(IPostTypeBLL BLL, ILogger<PostTypeController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostTypeCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostTypeUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("Available/{isAvalable}")]
        public async Task<ActionResult> GetIsAvailable(bool isAvalable)
        {
            var result = await _BLL.GetAllByIsAvailableAsync(isAvalable);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpGet("Paged")]
        public override async Task<ActionResult<IResponsePagedListDTO<PostTypeReadDTO>>> GetPaged([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetAllPagedAsync(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostTypeReadDTO>> Get(int id)
        {
            return await GetByIdAsync(id);
        }

        [ResponseCache(Duration = 10)]
        [HttpGet("Top/{top}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTop(int top)
        {
            var result = await _BLL.GetTopWithPostsAsync(top);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);
        }


    }
}
