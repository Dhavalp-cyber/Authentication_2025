using Authentication_2025.Data;
using Authentication_2025.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace Authentication_2025.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly DB mydb;

        public AdminPanelController(DB mydb)
        {
           this.mydb = mydb;
        }
        [Authorize]

        public IActionResult GetData()
        {
            IEnumerable<User> xData = mydb.Users;
            return View(xData);
        }


        [Authorize]
        public IActionResult UserLoginInfo()
        {
            var xData = mydb.loginlogoutiinfos
                            .Include(i => i.User)
                            .Where(i => i.User != null && i.User.Username != "Admin")
                            .OrderByDescending(i => i.LoginTime)
                            .ToList();

            return View(xData);
        }

        [Authorize]
        public IActionResult AdminLoginInfo()
        {
            var xData = mydb.loginlogoutiinfos
                            .Include(i => i.User)
                            .Where(i => i.User != null && i.User.Username == "Admin")
                            .OrderByDescending(i => i.LoginTime)
                            .ToList();

            return View(xData);
        }


        [Authorize]

        //public IActionResult LastLoginData()
        //{
        //    IEnumerable<LastLogin> xData = mydb.lastLogin;
        //    return View(xData);
        //}

        public IActionResult Edit(int? UserId)
        {
            if(UserId == null || UserId == 0)
            {
                return NotFound();
            }

            var obj = mydb.Users.Find(UserId);

            if(obj == null)
            {
                return NotFound();
            }
            obj.Password = DecryptPassword(obj.Password);
            List<User> users = new List<User>();
            users = (from username in mydb.Users select username).ToList();

            ViewBag.UserList = users;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Username = model.Username;
                    model.Email = model.Email;
                    model.Password = EncryptPassword(model.Password);
                    model.Mobile = model.Mobile;
                    model.Rolesname = model.Rolesname;
                    model.IsActive = model.IsActive;
                    mydb.Users.Update(model);
                    mydb.SaveChanges();

                    TempData["successMessage"] = "You Are eligible to Login , Plese Use Your Credential To Login";
                    return RedirectToAction("GetData");
                }
                else
                {
                    return View(model);
                }
            }
            catch(Exception)
            {
                throw;
            }


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

        public ActionResult ViewProfile(int UserId)
        {
            IEnumerable<User> xData = mydb.Users.ToList().Where(x => x.UserId == UserId);
            return View(xData);
        }
    }
}
