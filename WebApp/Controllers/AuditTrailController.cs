using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AuditTrailController : Controller
    {
        public IActionResult Index()
        {
            // Query to fetch all audit logs
            string query = "SELECT * FROM AuditTrail ORDER BY ActivityTimestamp DESC";
            DataTable auditLogs = CRUD.ExecuteQuery(query);

            return View(auditLogs);
        }
    }
}
