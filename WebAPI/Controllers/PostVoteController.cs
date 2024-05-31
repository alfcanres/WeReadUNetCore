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
    public class PostVoteController : Controller
    {
        private readonly IPostVoteBLL _BLL;
        private readonly ILogger<PostVoteController> _logger;

        public PostVoteController(IPostVoteBLL BLL, ILogger<PostVoteController> logger)
        {
            _BLL = BLL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PostVoteCreateDTO createModel)
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

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PostVoteUpdateDTO updateModel)
        {
            var response = await _BLL.UpdateAsync(updateModel.Id, updateModel);
            if (response.Validate.IsValid)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
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
