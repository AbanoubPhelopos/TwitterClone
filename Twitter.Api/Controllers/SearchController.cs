using Twitter.Contract.Users;

namespace Twitter.Api.Controllers
{
    [ApiController]
    [Route("api/search")]
    [Authorize]
    public class SearchController(ApplicationDbContext context, UserManager<User> userManager)
        : BaseController
    {
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            query = query.ToLower();

            var users = await context.Users
                .Where(u => u.FirstName.ToLower().Contains(query) || u.LastName.ToLower().Contains(query) || u.UserName.ToLower().Contains(query))
                .Select(u => new UserResponse(
                    u.Id,
                    //u.UserName,
                    u.FirstName,
                    u.LastName,
                    u.Image
                ))
                .ToListAsync();

            return Ok(users);
        }
    }
}