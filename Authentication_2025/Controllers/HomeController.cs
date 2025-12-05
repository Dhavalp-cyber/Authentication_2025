using Authentication_2025.Data;
using Authentication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Authentication_2025.Controllers
{
    public class HomeController : Controller
    {
        private readonly DB db;

        public HomeController(DB db)
        {

            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            var Username = Request.Cookies["Username"];
            var user = db.Users.FirstOrDefault(u => u.Username == Username);
            var userId = user.UserId;
            if (user == null)
            {
                return NotFound();
            }


            return RedirectToAction("Profile", "UserEdit", new { id = user.UserId }); ;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
