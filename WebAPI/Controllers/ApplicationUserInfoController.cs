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
    public class ApplicationUserInfoController : BaseController<ApplicationUserInfoCreateDTO, ApplicationUserInfoReadDTO, ApplicationUserInfoUpdateDTO>
    {
        private readonly IApplicationUserInfoBLL _BLL;
        private readonly ILogger<ApplicationUserInfoController> _logger;

        public ApplicationUserInfoController(IApplicationUserInfoBLL BLL, ILogger<ApplicationUserInfoController> logger) : base(BLL, logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ApplicationUserInfoCreateDTO createModel)
        {
            return await CreateAsync(createModel);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ApplicationUserInfoUpdateDTO updateModel)
        {
            return await UpdateAsync(updateModel.Id, updateModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await DeleteAsync(id);
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<IResponsePagedListDTO<ApplicationUserInfoListDTO>>> GetPaged([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetAllPagedAsync(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

        }


        [HttpGet("PagedByActive")]
        public async Task<ActionResult<IResponsePagedListDTO<ApplicationUserInfoListDTO>>> GetPagedByActive([FromQuery] PagerDTO pagerDTO)
        {
            var result = await _BLL.GetAllActivePagedAsync(pagerDTO);

            if (result.Validate.IsValid)
                return Ok(result);
            else
                return BadRequest(result);

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
