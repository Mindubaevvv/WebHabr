using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHabr.Data;
using WebHabr.Models;

namespace WebHabr.Controllers
{
    public class AuthorArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorArticleController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var articles = await _context.Articles.Where(a => a.AuthorId == userId).ToListAsync();
            return View(articles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            article.AuthorId = _userManager.GetUserId(User);
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
