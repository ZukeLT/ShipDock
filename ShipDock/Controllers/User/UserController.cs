using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;
namespace ShipDock.Controllers.UserNamespace
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult OpenRegisterUser()
        {
            return View("RegisterUser");
        }

        [HttpPost]
        public IActionResult RegisterTheUser(User model)
        {
            if (ModelState.IsValid)
            {
                // Insert user registration logic here
                bool isRegistered = RegisterUser(model);

                if (isRegistered)
                {
                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Registration failed. Please try again.";
                }
            }

            return View("RegisterUser", model);
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public IActionResult LogTheUserIn(string username, string password)
        {
            // Insert user login logic here
            (bool isAuthenticated, string userRole, string pwd) = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("UserRole", userRole);
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Login failed. Please try again.";
                return View("UserLogin");
            }
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult OpenRemindPassword()
        {
            return View("RemindPassword");
        }

        [HttpPost]
        public IActionResult RemindPassword(string username)
        {
            string password = GetPasswordByUsername(username);
            if (!string.IsNullOrEmpty(password))
            {
                ViewBag.Password = password;
            }
            else
            {
                ViewBag.Password = "Username not found";
            }

            return View("RemindPassword");
        }

        private string GetPasswordByUsername(string username)
        {
            string sql = $"SELECT Password FROM [User] WHERE Username = '{username}'";
            DataView dw = (DataView)DataSource.ExecuteSelectSQL(sql);

            if (dw != null && dw.Table.Rows.Count > 0)
            {
                return dw.Table.Rows[0]["Password"].ToString();
            }
            else
            {
                return null;
            }
        }

        private bool RegisterUser(User user)
        {
            string sql = $"INSERT INTO [User] (Username, Password, UserRole) VALUES ('{user.Username}', '{user.Password}', 'Klientas')";
            return DataSource.UpdateDataSQL(sql);
        }

        private (bool isAuthenticated, string userRole, string pwd) AuthenticateUser(string username, string password)
        {
            string sql = $"SELECT * FROM [User] WHERE Username = '{username}' AND Password = '{password}'";
            DataView dw = (DataView)DataSource.ExecuteSelectSQL(sql);

            if (dw != null && dw.Table.Rows.Count > 0)
            {
                // Assuming the user role is stored in the "UserRole" column of the "User" table
                string userRole = dw.Table.Rows[0]["UserRole"].ToString();
                string pwd = dw.Table.Rows[0]["Password"].ToString();
                return (true, userRole, pwd);
            }
            else
            {
                return (false, null, null);
            }
        }
    }
}
