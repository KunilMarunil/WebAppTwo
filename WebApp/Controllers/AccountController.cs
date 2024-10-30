using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Query to check if user exists with matching username and password hash
            string query = "SELECT 1 FROM Users WHERE Username = @Username AND PasswordHash = HASHBYTES('SHA2_256', @Password)";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.VarChar){ Value = model.Username },
                new SqlParameter("@Password", SqlDbType.VarChar){ Value = model.Password },
            };

            var result = CRUD.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 1)
            {
                // Create claims for user identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.NameIdentifier, result.Rows[0]["UserId"].ToString())
                };

                // Create claims identity and sign in
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if the username or email already exists
            string checkUserQuery = "SELECT COUNT(1) FROM Users WHERE Username = @Username OR Email = @Email";
            SqlParameter[] checkParams = {
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Email", model.Email)
            };

            var existingUser = CRUD.ExecuteQuery(checkUserQuery, checkParams);
            if ((int)existingUser.Rows[0][0] > 0)
            {
                ModelState.AddModelError(string.Empty, "Username or Email already exists.");
                return View(model);
            }

            // Insert new user into Users table with hashed password
            string insertUserQuery = @"
                INSERT INTO Users (Username, PasswordHash, FullName, Email, CreatedDate, IsActive)
                VALUES (@Username, HASHBYTES('SHA2_256', @Password), @FullName, @Email, GETDATE(), 1)";
            SqlParameter[] insertParams = {
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password), // Hashes the password during insertion
                new SqlParameter("@FullName", model.FullName),
                new SqlParameter("@Email", model.Email)
            };

            CRUD.ExecuteNonQuery(insertUserQuery, insertParams);

            // Redirect to the login page after successful registration
            return RedirectToAction("Login", "Account");
        }

        return View(model); // Return to the registration form if there are validation errors
    }
}
