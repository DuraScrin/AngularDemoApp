using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public IActionResult GetSecret()
        {
            return Unauthorized();
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null)
                return NotFound();
            else
                return NotFound(thing);
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return StatusCode(500, thingToReturn);
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return StatusCode(400, "This is a bad requist.");
        }
    }
}