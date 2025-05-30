using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using UserManagementAPI.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace UserManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var path = RequestLoggingMiddleware.GetLogFilePath();
            if (!System.IO.File.Exists(path))
                return NotFound("Log file not found.");

            var logs = await System.IO.File.ReadAllTextAsync(path);
            return Content(logs, "text/plain");
        }
    }
}