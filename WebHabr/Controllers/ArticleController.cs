using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHabr.Data;
using WebHabr.Models;

namespace WebHabr.Controllers
{
    [Authorize(Roles = "User, Author, Admin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ArticleController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.ToListAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Comments)
                .Include(a => a.Ratings)
                .FirstOrDefaultAsync(a => a.Id == id);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int articleId, string content)
        {
            var comment = new Comment
            {
                ArticleId = articleId,
                Content = content,
                UserId = _userManager.GetUserId(User)
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = articleId });
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(int articleId, int value)
        {
            var rating = new Rating
            {
                ArticleId = articleId,
                Value = value,
                UserId = _userManager.GetUserId(User)
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = articleId });
        }
    }
}
