using Authentication_2025.Data;
using Authentication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_2025.Controllers
{
    public class mainController : Controller
    {
        private readonly DB context;

        public mainController(DB context)
        {
            this.context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var loggedUser = User.Identity.Name;


            if (loggedUser == "Admin")
            {
                var allData = context.fruits.ToList();
                return View(allData);
            }

            var userData = context.fruits
                            .Where(x => x.UserOfFruit == loggedUser)
                            .ToList();

            return View(userData);
        }

        public IActionResult Create()
        {
            //List<User> users = new List<User>();
            //users = (from username in context.Users select username).ToList();

            //ViewBag.UserList = users;
            ViewBag.UserList = context.Users
       .Select(u => new { u.UserId, u.Username })
       .ToList();

            return View(new Fruit_Table());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Fruit_Table fruit)
        {
            if (ModelState.IsValid)
            {
                context.fruits.Add(fruit);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(fruit);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = context.fruits.Find(id);

            if (obj == null)
            {
                return NotFound(obj);
            }

            ViewBag.UserList = context.Users
       .Select(u => new { u.UserId, u.Username })
       .ToList();


            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Fruit_Table obj)
        {
            if (ModelState.IsValid)
            {


                if (obj == null)
                {
                    return NotFound();
                }

                context.fruits.Update(obj);
                context.SaveChanges();

                return RedirectToAction("Index");

            }
            return View(obj);

        }

        public IActionResult view(int id)
        {
            IEnumerable<Fruit_Table> xData = context.fruits.Where(x => x.Id == id).ToList();
            return View(xData);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = context.fruits.Find(id);

            if (obj == null)
            {
                return NotFound(obj);
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int? id)
        {
            try
            {
                var obj = context.fruits.Find(id);

                if (obj == null)
                {
                    return NotFound(obj);
                }
                if (ModelState.IsValid)
                {
                    context.fruits.Remove(obj);
                    context.SaveChanges();
                }


                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
