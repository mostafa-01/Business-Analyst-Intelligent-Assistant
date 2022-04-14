using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAIA.Data;
using BAIA.Models;
using Microsoft.AspNetCore.Cors;

namespace BAIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class UsersController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public UsersController(BAIA_DB_Context context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------//

        // READ
      

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/youssef@gmail.com/youssef12345
        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<User>> GetUser(string email , string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (user == null)
            {
                return NoContent();
            }

            return user;
        }

        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified; // Used to detect changes and apply it if found

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user); // Return new User location (api/Users/idOfNewUser) in the header
        }

        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //-----------------------------------------------------------------------//


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
