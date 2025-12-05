using Authentication_2025.Data;
using Authentication_2025.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Authentication_2025.Controllers
{
    public class UserEditController : Controller
    {
        private readonly DB db;
        public UserEditController(DB db)
        {
            this.db = db;

        }
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;

            }
            else
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }

        }

        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedPassword = Convert.FromBase64String(password);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                return decryptedPassword;
            }
        }
        public IActionResult Profile(int id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Password = DecryptPassword(user.Password);

            var model = new UserEditViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = EncryptPassword(user.Password)
            };

            return View(model);

        }
        [HttpPost]
        public IActionResult Profile(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.Password =EncryptPassword(model.Password);  

                db.Users.Update(user);
                db.SaveChanges();

                return RedirectToAction("Index", "main");
            }
            return View(model);


        }
    }
}
