using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace UserManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>();
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(users.Values);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            if (!users.TryGetValue(id, out var user))
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.EmailAddress = user.EmailAddress.Trim().ToLowerInvariant();

            if (users.Values.Any(u => u.EmailAddress == user.EmailAddress))
                return Conflict("A user with this email address already exists.");

            user.UserId = Interlocked.Increment(ref nextId);
            if (!users.TryAdd(user.UserId, user))
            {
                return StatusCode(500, "Could not add user due to a concurrency issue.");
            }
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            if (!users.ContainsKey(id))
                return NotFound();

            updatedUser.UserId = id;
            if (!users.TryUpdate(id, updatedUser, users[id]))
            {
                return StatusCode(500, "Could not update user due to a concurrency issue.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!users.TryRemove(id, out _))
                return NotFound();
            return NoContent();
        }
    }
}