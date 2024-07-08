using BusinessLogicLayer.BusinessObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using DataTransferObjects.DTO;
using DataTransferObjects.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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
        private readonly IValidate _validateDTO;

        public AccountController(IAccountBLL accountBLL, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _accountBLL = accountBLL;
            _logger = logger;
            _configuration = configuration;
            _validateDTO = new ValidateDTO();
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Post([FromBody] UserCreateDTO createModel)
        {
            try
            {
                var validate = await _accountBLL.ValidateInsertAsync(createModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }
                var response = await _accountBLL.InsertAsync(createModel);

                if(!response.ValidateDTO.IsValid)
                {
                    return BadRequest(response.ValidateDTO);
                }
                else
                {
                    return Ok(response);
                }

                
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "INSERT OPERATION : {createDTO}", createModel);

                return StatusCode(500, _validateDTO);
            }

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserSignInDTO userSignInDTO)
        {

            try
            {

                var response = await _accountBLL.SignInAsync(userSignInDTO);
                if(response.IsValid)
                {

                    var userResponse = await _accountBLL.GetByUserNameOrEmail(userSignInDTO.Email);

                    TokenResponseModel tokenResponse = new TokenResponseModel(
                    userResponse,
                    DateTime.Now,
                    _configuration);


                    return Ok(tokenResponse);
                }
                else
                {
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "Login OPERATION : {userSignInDTO}", userSignInDTO);

                return StatusCode(500, _validateDTO);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserUpdatePasswordDTO updateModel)
        {
            try
            {
                var validate = await _accountBLL.ValidateUpdatePasswordAsync(updateModel);
                if (!validate.IsValid)
                {
                    return BadRequest(validate);
                }

                await _accountBLL.UpdatePasswordAsync(updateModel);

                return Ok();
            }
            catch (Exception ex)
            {
                string friendlyError = FriendlyErrorMessages.ErrorOnReadOpeation;
                _validateDTO.AddError(friendlyError);
                _logger.LogError(ex, "ChangePassword OPERATION : {updateDTO}", updateModel);

                return StatusCode(500, _validateDTO);
            }
        }
    }
}
