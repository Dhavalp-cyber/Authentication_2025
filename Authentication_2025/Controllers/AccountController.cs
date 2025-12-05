using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Authentication_2025.Models;
using System.Security.Claims;
using System.Text;
using Authentication_2025.Data;

namespace Authentication_2025.Controllers
{
    public class AccountController : Controller
    {
        private readonly DB context;
        public AccountController(DB context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {

                var data = context.Users.Where(e => e.Username == model.Username).SingleOrDefault();



                if (data != null)
                {

                    bool isValid = (data.Username == model.Username && DecryptPassword(data.Password) == model.Password && data.IsActive == true);
                    if (isValid)
                    {

                        CookieOptions opt = new CookieOptions();
                        opt.Expires = DateTime.Now.AddHours(1);

                        Response.Cookies.Append("IsLogin", "true", opt);
                        Response.Cookies.Append("Username", model.Username, opt);


                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username)
                        //, new Claim(ClaimTypes.Role, model.Rolesname)
                        },
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
                        HttpContext.Session.SetString("Username", model.Username);

                       
                        // HttpContext.Session.SetString("Rolesname", model.Rolesname);


                        //var mydata = new LastLogin()
                        //{
                        //    userLoggedin = model.Username,
                        //    lastLogin = DateTime.Now,

                        //};
                        //context.lastLogin.Add(mydata);

                        context.SaveChanges();
                        return RedirectToAction("Index", "main");
                    }
                    else if (data.IsActive == false)
                    {
                        TempData["errorAccountInactive"] = "Account Not Active !!!!";
                    }
                    else
                    {


                        TempData["errorPassword"] = "Invalid Password";
                        return View(model);
                    }

                }
                else
                {
                    TempData["errorUsername"] = "username not found";
                }
            }
            else
            {
                return View(model);
            }
            return View(model);
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
        public IActionResult LogOut()
        {
            //var data = context.Users.Where(e => e.Username == model1.Username).SingleOrDefault();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            //var mydata = new LastLogin()
            //{
            //    userLoggedin = User.Identity.Name,
            //    lastLogOut = DateTime.Now,

            //};
            //context.lastLogin.Add(mydata);
            context.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = EncryptPassword(model.Password),
                    Mobile = model.Mobile,
                    Rolesname = model.Roles,
                    IsActive = model.IsActive,
                };
                context.Users.Add(data);
                context.SaveChanges();
                TempData["successMessage"] = "You are eligible to login , Please use your credentials to login";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "Empty form can't be submitted";
                return View(model);

            }
        }
        [AcceptVerbs("Post", "Get")]
        public IActionResult UserNameIsExist(string userName)
        {
            var data = context.Users.Where(e => e.Username == userName).SingleOrDefault();
            if (data != null)
            {
                return Json($"Username {userName} already exists");
            }
            else
            {
                return Json(true);
            }

            return View();
        }

    }
}
