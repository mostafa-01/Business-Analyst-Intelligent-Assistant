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
using System.Text.Json;
using RestSharp;

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

        // GET: api/Users/GetProjectTitles/1
        // This API returns Project Titles related to user with {id}
        [Route("api/Users/GetProjectTitles")]
        [HttpGet("GetProjectTitles/{id}")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<string>>> GetProjectTitles(int id)
        {

            var user = await _context.Users.Include(p => p.Projects).FirstOrDefaultAsync(x => x.UserID == id);
            if (user == null)
                return NoContent();
            else
            {
                List<string> ProjectTitles = new List<string>();
                var projects = user.Projects.ToList();
                foreach (Project project in projects)
                {
                    ProjectTitles.Add(project.ProjectTitle);
                }
                return ProjectTitles;
            }
        }

        /*
        //Get: Test API
        [Route("api/Users/Test")]
        [HttpGet("Test")]
        public async Task<ActionResult<string>> Test()
        {
            var client = new RestClient($"http://127.0.0.1:5000/");
            var request = new RestRequest("", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }
        */

        // GET: api/Users/GetAllUsers
        // This API returns all Users in Database
        [Route("api/Users/GetAllUsers")]
        [HttpGet("GetAllUsers")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users//GetUser/5
        // This API returns data related to User with {id}
        [Route("api/Users/GetUser")]
        [HttpGet("GetUser/{id}")]
        [EnableCors]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // Post: api/Users/Login
        // This API checks if User exists in Database by Email and Password from Body
        // { "Email":"youssef@gmail.com", "Password":"youssef12345" }
        [Route("api/Users/Login")]
        [HttpPost("Login")]
        [EnableCors]
        public async Task<ActionResult<User>> Login([FromBody] LoginModel model)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

            if (user == null)
            {
                return NoContent();
            }

            return user;
        }

        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/Users/UpdateUser/1
        // This API updates data related to User with {id}
        // Must send User Object in Body
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Users/UpdateUser")]
        [HttpPut("UpdateUser/{id}")]
        [EnableCors]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {

            if (id != user.UserID)
            {
                return BadRequest();
            }

            /*string emailBefore = _context.Users.FirstOrDefault(x => x.UserID == id).Email;
            string emailAfter = user.Email;
            if (emailAfter != emailBefore)
            {
                var newEmailAlreadyExist = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (newEmailAlreadyExist!= null)
                {
                    return BadRequest();
                }


            }*/
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

            var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);
            return updatedUser;
        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/Users/PostUser
        // This API creates a new User
        // Must send User Object in Body
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Users/PostUser")]
        [HttpPost("PostUser")]
        [EnableCors]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if(_context.Users.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                return BadRequest();
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user); // Return new User location (api/Users/GetUser/idOfNewUser) in the header
        }

        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/Users/DeleteUser/1
        // This API deletes a specific User with {id}
        [Route("api/Users/DeleteUser")]
        [HttpDelete("DeleteUser/{id}")]
        [EnableCors]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //-----------------------------------------------------------------------//


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
