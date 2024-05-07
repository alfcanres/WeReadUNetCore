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
    public class MoodTypeController : CustomControllerBase<MoodTypeCreateDTO, MoodTypeReadDTO, MoodTypeUpdateDTO>
    {
        private readonly IMoodTypeBLL _BLL;
        private readonly ILogger<MoodTypeController> _logger;

        public MoodTypeController(IMoodTypeBLL BLL, ILogger<MoodTypeController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MoodTypeCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] MoodTypeUpdateDTO updateModel)
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
        public override async Task<ActionResult<IResponsePagedListDTO<MoodTypeReadDTO>>> GetPaged([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetAllPagedAsync(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("{id}")]
        public async Task<ActionResult<MoodTypeReadDTO>> Get(int id)
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
