using DataTransferObjects.DTO;
using DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Client.Repository.Post;
using WebAPI.Client.ViewModels;

namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        private readonly ILogger<PostController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(
            IPostRepository repository,
            ILogger<PostController> logger,
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


            var response = await _repository.GetPublishedPagedAsync(pager);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(response);
        }

        public IActionResult Create()
        {
            return View(new PostCreateDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDTO createModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createModel);
            }


            var response = await _repository.CreateAsync(createModel);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                foreach (var error in response.MessageList)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(createModel);
            }
        }


        public async Task<IActionResult> Edit(int id)
        {


            var response = await _repository.GetByIdAsync(id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            PostUpdateDTO PostUpdateDTO = new PostUpdateDTO
            {
                Id = response.Content.Id,
                ApplicationUserId = response.Content.ApplicationUserId,
                MoodTypeId  = response.Content.MoodTypeId,
                PostTypeId = response.Content.PostTypeId,
                Title = response.Content.Title,
                Text = response.Content.Text,

            };


            return View(PostUpdateDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PostUpdateDTO editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            var response = await _repository.UpdateAsync(editModel);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "Post");
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
        public async Task<ActionResult> Delete(PostUpdateDTO editModel)
        {


            var response = await _repository.DeleteAsync(editModel.Id);

            if (response.Status == ResponseStatus.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (response.Status == ResponseStatus.Success)
            {
                return RedirectToAction("Index", "Post");
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
