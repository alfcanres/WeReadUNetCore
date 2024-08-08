using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PostCommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
