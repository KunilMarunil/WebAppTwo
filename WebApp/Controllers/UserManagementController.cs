using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            // Fetch all users from the database to display
            string query = "SELECT * FROM Users";
            DataTable users = CRUD.ExecuteQuery(query);

            return View(users);
        }

        public IActionResult Edit(int id)
        {
            // Fetch user details by ID to edit
            string query = "SELECT * FROM Users WHERE UserId = @UserId";
            SqlParameter[] parameters = { new SqlParameter("@UserId", id) };
            DataTable userTable = CRUD.ExecuteQuery(query, parameters);

            if (userTable.Rows.Count == 0)
            {
                return NotFound();
            }

            // Map DataRow to UserViewModel if needed
            var userViewModel = new UserViewModel
            {
                UserId = id,
                Username = userTable.Rows[0]["Username"].ToString(),
                FullName = userTable.Rows[0]["FullName"].ToString(),
                Email = userTable.Rows[0]["Email"].ToString(),
                IsActive = (bool)userTable.Rows[0]["IsActive"]
            };

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update user details in the database
                string updateQuery = @"
                UPDATE Users 
                SET Username = @Username, FullName = @FullName, Email = @Email, IsActive = @IsActive 
                WHERE UserId = @UserId";
                SqlParameter[] parameters = {
                new SqlParameter("@UserId", model.UserId),
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@FullName", model.FullName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@IsActive", model.IsActive)
            };

                CRUD.ExecuteNonQuery(updateQuery, parameters);

                // Log the audit trail
                CRUD.LogAuditTrail(
                    userId: model.UserId,
                    username: model.Username,
                    menuAccessed: "User Management",
                    activity: "Edited User",
                    dataChanges: $"Updated user {model.UserId} - Username: {model.Username}, FullName: {model.FullName}, Email: {model.Email}, IsActive: {model.IsActive}"
                );

                return RedirectToAction("Index");
            }

            return View(model); // Return the same view if validation fails
        }
    }
}