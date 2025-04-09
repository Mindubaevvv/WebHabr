using Microsoft.AspNetCore.Mvc;

namespace WebHabr.Controllers
{
    public class AuthorArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
