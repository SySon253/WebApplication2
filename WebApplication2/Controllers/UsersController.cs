using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Service;

namespace WebApplication2.Controllers
{
    [Route("User")]
    public class UsersController : Controller
    {
        private UserService userService;

        public UsersController(UserService _userService)
        {
            userService = _userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[Route("")]
        //[Route("Login")]
        //public IActionResult Login()
        //{
        //    return View("Login");
        //}
        //[HttpPost]
        //[Route("Login")]
        //public IActionResult Login(string useremail, string userpassword)
        //{
        //    if (userService.Login(useremail, userpassword))
        //    {
        //        var user = userService.findByEmail(useremail);
        //        HttpContext.Session.SetString("email", useremail);
        //        HttpContext.Session.SetString("role", user.UserRole ?? "");
        //        return RedirectToAction("Welcome");
        //    }
        //    else
        //    {
        //        TempData["Msg"] = "Failed";
        //        return RedirectToAction("Login");
        //    }
        //}
        [Route("")]
        [Route("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string useremail, string userpassword)
        {
            // Kiểm tra đăng nhập qua service
            if (userService.Login(useremail, userpassword))
            {
                // Lấy thông tin user
                var user = userService.findByEmail(useremail);

                // --- TẠO DANH TÍNH (CLAIMS) ---
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserEmail),
                    new Claim("FullName", user.UserName ?? ""),
                    new Claim(ClaimTypes.Role, user.UserRole ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                // --- CẤU HÌNH COOKIE ---
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Giữ đăng nhập sau khi đóng trình duyệt
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Cookie hết hạn sau 30 phút
                };

                // --- ĐĂNG NHẬP (ghi cookie vào trình duyệt) ---
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                // Chuyển hướng đến trang Welcome
                return RedirectToAction("Welcome");
            }
            else
            {
                TempData["Msg"] = "Đăng nhập thất bại!";
                return RedirectToAction("Login");
            }
        }
        [Route("Welcome")]
        public IActionResult Welcome()
        {
            ViewBag.email = HttpContext.Session.GetString("email");
            return View("Welcome");
        }
        //[Route("Logout")]
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Remove("email");
        //    return RedirectToAction("Login");
        //}
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View("Register", new User());
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(User user)
        {
            // --- 1. Kiểm tra mật khẩu trùng khớp ---
            if (user.UserPassword != user.ConfirmPassword)
            {
                TempData["Msg"] = "Password and Confirm Password do not match!";
                return RedirectToAction("Register");
            }

            // --- 2. Kiểm tra dữ liệu hợp lệ ---
            if (ModelState.IsValid)
            {
                // --- 3. Gán quyền (role) ---
                List<string> roles = new List<string>();
                if (user.IsAdmin) roles.Add("Admin");
                if (user.IsUser) roles.Add("User");
                user.UserRole = string.Join(",", roles);

                // --- 4. Mã hóa mật khẩu ---
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);

                // --- 5. Lưu vào cơ sở dữ liệu ---
                if (userService.Create(user))
                {
                    // --- 6. Sau khi tạo thành công -> tự động đăng nhập ---
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserEmail),
                        new Claim("FullName", user.UserName ?? ""),
                        new Claim(ClaimTypes.Role, user.UserRole ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // --- 7. Chuyển sang trang Welcome ---
                    return RedirectToAction("Welcome");
                }
                else
                {
                    TempData["Msg"] = "Registration failed!";
                    return RedirectToAction("Register");
                }
            }

            // --- 8. Trường hợp dữ liệu không hợp lệ ---
            TempData["Msg"] = "Invalid data!";
            return RedirectToAction("Register");
        }

            //[HttpGet]
            //[Route("Register")]
            //public IActionResult Register()
            //{
            //    return View("Register", new User());
            //}
            //[HttpPost]
            //[Route("Register")]
            //public IActionResult Register(User user)
            //{
            //    if (user.UserPassword != user.ConfirmPassword)
            //    {
            //        TempData["Msg"] = "Password and Confirm Password do not match!";
            //        return RedirectToAction("Register");
            //    }
            //    if (ModelState.IsValid)
            //    {
            //        List<string> roles = new List<string>();
            //        if (user.IsAdmin) roles.Add("Admin");
            //        if (user.IsUser) roles.Add("User");

            //        user.UserRole = string.Join(",", roles);

            //        user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
            //        if (userService.Create(user))
            //        {
            //            return RedirectToAction("Login");
            //        }
            //        else
            //        {
            //            TempData["Msg"] = "Failed";
            //            return RedirectToAction("Register");
            //        }

            //    }
            //    TempData["Msg"] = "Dữ liệu không hợp lệ!";
            //    return RedirectToAction("Register");

            //}




            //[HttpGet]
            //[Route("Profile")]
            //public IActionResult Profile()
            //{
            //    var user = userService.findByEmail(HttpContext.Session.GetString("email"));
            //    return View("Profile", new User());
            //}
            //[HttpPost]
            //[Route("Profile")]
            //public IActionResult Profile(User user)
            //{
            //    var currentUser = userService.findByEmail(user.UserEmail);
            //    user.UserId = currentUser.UserId;
            //    if (string.IsNullOrEmpty(user.UserPassword))
            //    {
            //        user.UserPassword = currentUser.UserPassword;
            //    }
            //    else
            //    {
            //        user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
            //    }
            //    if (userService.Update(user))
            //    {
            //        TempData["Msg"] = "Success";
            //    }
            //    else
            //    {
            //        TempData["Msg"] = "Failed";
            //    }
            //    return RedirectToAction("profile");
            //}
        }
}
