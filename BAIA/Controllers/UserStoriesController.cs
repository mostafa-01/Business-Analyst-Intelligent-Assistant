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
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json;

namespace BAIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]

    public class UserStoriesController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public UserStoriesController(BAIA_DB_Context context)
        {
            _context = context;
        }

        // GET: api/UserStories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserStory>>> GetUserStories()
        {
            return await _context.UserStories.ToListAsync();
        }

        // GET: api/UserStories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserStory>> GetUserStory(int id)
        {
            var userStory = await _context.UserStories.FindAsync(id);

            if (userStory == null)
            {
                return NotFound();
            }

            return userStory;
        }

        /// <summary>
        /// this api is made to find and return all the User Stories related to a specific project 
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns>Ilist<UserStories></UserStories></returns>
        [Route("api/UserStories/getProjectUserStories")]
        [HttpGet("getProjectUserStories/{id}")]
        [EnableCors]
        public async Task<ActionResult<List<UserStory>>> getProjectUserStories(int id)
        {
            Project project = await _context.Projects
                .Include(m => m.Meetings)
                .ThenInclude(us => us.UserStories)
                .FirstOrDefaultAsync(p => p.ProjectID == id);
            
            if (project == null)
                return StatusCode(204 , "Project not found");

            try{

                return Ok(project.Meetings.SelectMany(us => us.UserStories).ToList());

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// GenerateManully is the usrstories endpoint to generate and save the new user stories in the database
        /// sequence: Client app requests a to generate USs manually from a specidic service so,
        /// the api tends to get all the servicce details from the database related to this service
        /// and request the flask endpoint for userstories generation with the list of details
        /// the response is the list of user stories description
        /// </summary>
        /// <param name="GenerateUSModel {projectid , serviceid , filepath}"></param>
        /// <returns>statuscode(500(InternalServError) , 200(Ok) , 204(NoContent))<UserStories></UserStories></returns>
        [Route("api/UserStories/GenerateUS")]
        [HttpGet("GenerateUS")]
        [EnableCors]
        public async Task<ActionResult> GenerateUS([FromBody] GenerateUSModel model)
        {
            Project pj =await _context.Projects
                .Include(m => m.Meetings)
                .ThenInclude(s => s.Services)
                .ThenInclude(d => d.ServiceDetails)
                .FirstOrDefaultAsync(p => p.ProjectID == model.ProjectID);
            if (pj == null)
            {
                return NoContent();
            }
            else
            {
                try
                {
                    var services = pj.Meetings.SelectMany(s => s.Services).ToList();
                    var selectedService = services.FirstOrDefault(s => s.ServiceID == model.ServiceID);

                    var details = selectedService.ServiceDetails.Select(d => d.ServiceDetailString).ToList(); // test on more than one detail

                    var client = new RestClient($"http://127.0.0.1:5000/");
                    var request = new RestRequest("userstories", Method.Post);
                    request.AddJsonBody(new
                    {
                        services = details,
                        filepath = model.Filepath
                    });
                    RestResponse response = await client.ExecuteAsync(request);
                    if (response.Content == null)
                        return NoContent();
                    
                    var UserStoriesDescriptions = JsonConvert
                    .DeserializeObject<UserStoryResponseModel>(response.Content);

                    List<UserStory> US = new List<UserStory>();

                    string preconditions = " ";
                    int i = 1;
                    foreach (var pr in UserStoriesDescriptions.preconditions)
                    {
                        if(UserStoriesDescriptions.preconditions.Count == i)
                            preconditions.Concat(pr);
                        else
                            preconditions.Concat(pr + '#');

                        i++;
                    }

                    string AccCrieteria = " ";
                    i = 1;
                    foreach (var ac in UserStoriesDescriptions.acceptanceCriteria)
                    {
                        if (UserStoriesDescriptions.preconditions.Count == i)
                            AccCrieteria.Concat(ac);
                        else
                            AccCrieteria.Concat(ac + '#');

                        i++;
                    }

                    foreach (var us in UserStoriesDescriptions.stories)
                    {
                        US.Add(new UserStory{
                            UserStoryTitle = selectedService.ServiceTitle,
                            UserStoryDescription = us,
                            Preconditions = preconditions,
                            AcceptanceCriteria = AccCrieteria
                        });
                    }

                    _context.UserStories.AddRange(US);
                    await _context.SaveChangesAsync();
                    return Ok();
                }catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
        }

        // PUT: api/UserStories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserStory(int id, UserStory userStory)
        {
            if (id != userStory.UserStoryID)
            {
                return BadRequest();
            }

            _context.Entry(userStory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserStoryExists(id))
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/UserStories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<UserStory>> PostUserStory(UserStory userStory)
        {
            _context.UserStories.Add(userStory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserStory", new { id = userStory.UserStoryID }, userStory);
        }

        // DELETE: api/UserStories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserStory(int id)
        {
            var userStory = await _context.UserStories.FindAsync(id);
            if (userStory == null)
            {
                return NotFound();
            }

            _context.UserStories.Remove(userStory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserStoryExists(int id)
        {
            return _context.UserStories.Any(e => e.UserStoryID == id);
        }
    }
}
