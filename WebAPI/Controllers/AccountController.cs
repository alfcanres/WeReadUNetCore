using BusinessLogicLayer.BusinessObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBLL _accountBLL;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountBLL accountBLL, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _accountBLL = accountBLL;
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Post([FromBody] UserCreateDTO createModel)
        {
            try
            {
                IResponseDTO<UserReadDTO> responseDTO = await _accountBLL.InsertAsync(createModel);

                if (responseDTO.Validate.IsValid)
                {
                    return Ok(responseDTO);
                }
                else
                {
                    return BadRequest(responseDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserSignInDTO userSignInDTO)
        {
            try
            {
                TokenResponseModel tokenResponse = new TokenResponseModel(
                await _accountBLL.SignInAsync(userSignInDTO),
                DateTime.Now,
                _configuration);

                if (tokenResponse.Validate.IsValid)
                {
                    return Ok(tokenResponse);
                }
                else
                {
                    return BadRequest(tokenResponse);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserUpdatePasswordDTO updateModel)
        {
            try
            {

                IValidate response = await _accountBLL.UpdatePasswordAsync(updateModel);

                if (response.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something whent wrong, please try again later");
            }
        }
    }
}
