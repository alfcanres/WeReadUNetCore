using DataTransferObjects.DTO;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.ViewModels;

namespace WebApp.Controllers
{
    public class ApplicationUserInfoController : Controller
    {
        private readonly IApplicationUserInfoRepository _repository;
        private readonly ILogger<ApplicationUserInfoController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUserInfoController(
            IApplicationUserInfoRepository repository,
            ILogger<ApplicationUserInfoController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            string token = Convert.ToString(_httpContextAccessor.HttpContext.Request.Cookies["AuthToken"]);
            _repository.SetBearerToken(token);
        }

        public async Task<IActionResult> Index(PagerParams pager = null)
        {


            var response = await _repository.GetPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public async Task<IActionResult> View(int id)
        {

            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }


            return View(response.Content);
        }


        public async Task<IActionResult> Delete(int id)
        {


            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response.Content);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ApplicationUserInfoUpdateDTO editModel)
        {


            var response = await _repository.DeleteAsync(editModel.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "ApplicationUserInfo");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(editModel);
            }
        }

    }
}
