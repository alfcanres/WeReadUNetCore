using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using DataTransferObjects;
using DataTransferObjects.DTO;
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
        public async Task<ActionResult> GetPaged([FromQuery] PagerParams pagerDTO)
        {

            try
            {
                var response = await _BLL.GetAllPagedAsync(pagerDTO);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                base._validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PAGED", pagerDTO);

                return StatusCode(500, _validateDTO);
            }
        }


        [HttpGet("PagedByActive")]
        public async Task<ActionResult> GetPagedByActive([FromQuery] PagerParams pagerDTO)
        {


            try
            {
                var response = await _BLL.GetAllActivePagedAsync(pagerDTO);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                base._validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET PagedByActive", pagerDTO);

                return StatusCode(500, _validateDTO);
            }

        }

        [ResponseCache(Duration = 10)]
        [HttpGet("Top/{top}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTop(int top)
        {
            var result = await _BLL.GetTopWithPostsAsync(top);

            try
            {
                var response = await _BLL.GetTopWithPostsAsync(top);

                return Ok(response);

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                base._validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "GET Top", top);

                return StatusCode(500, _validateDTO);
            }
        }
    }
}
