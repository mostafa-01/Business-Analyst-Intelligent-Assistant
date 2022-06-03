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

    public class ProjectsController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public ProjectsController(BAIA_DB_Context context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------//

        // READ

        // GET: api/Projects/GetAllProjects
        // This API returns all Projects in Database
        [Route("api/Projects/GetAllProjects")]
        [HttpGet("GetAllProjects")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/GetMeetingTitles/4
        // This API returns all Meeting Titles in Database with Project {id}
        [Route("api/Projects/GetMeetingTitles")]
        [HttpGet("GetMeetingTitles/{id}")]
        [EnableCors]
        public async Task<ActionResult<List<string>>> GetMeetingTitles(int id)
        {
            var project = await _context.Projects.Include(p => p.Meetings).FirstOrDefaultAsync(x => x.ProjectID == id);
            if (project == null)
                return NoContent();
            else
            {
                try
                {

                    List<string> MeetingTitles = new List<string>();
                    var meetings = project.Meetings.ToList();
                    foreach (Meeting meeting in meetings)
                    {
                        MeetingTitles.Add(meeting.MeetingTitle);
                    }
                    return MeetingTitles;

                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.ToString());
                    return StatusCode(500);
                }
            }
        }


        //Get: api/Projects/GenerateAsIs/4
        //this api returns all the verified services and its detalis in a list of tuples --> [{serviceTitle1 , {deatails1 , details2}}]
        [Route("api/Projects/GenerateAsIs")]
        [HttpGet("GenerateAsIs/{id}")]
        [EnableCors]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GenerateAsIs(int id)
        {
            var project = await _context.Projects
                .Include(m => m.Meetings)
                .ThenInclude(s => s.Services)
                .ThenInclude(d => d.ServiceDetails)
                .FirstOrDefaultAsync(p => p.ProjectID == id);
            if (project == null)
                return NoContent();
            else
            {
                try
                {

                    // Tuple<string, List<string>> listnode = new Tuple<string,List<string>>; 
                    Dictionary<string, List<string>> AsIs = new Dictionary<string, List<string>>();
                    foreach (Meeting M in project.Meetings)
                    {
                        foreach (Service S in M.Services)
                        {
                            List<string> details = new List<string>();
                            if (S.ServiceVerified == true)
                            {
                                foreach (var SD in S.ServiceDetails)
                                {
                                    details.Add(SD.ServiceDetailString);
                                }


                                AsIs.Add(S.ServiceTitle, details);
                            }
                        }
                    }
                    return AsIs;
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.ToString());
                    return StatusCode(500);
                }
            }
        } 


        // GET: api/Projects/GetProject/5
        // This API returns Project with {id} including it's Meetings
        [Route("api/Projects/GetProject")]
        [HttpGet("GetProject/{id}")]
        [EnableCors]
        public async Task<ActionResult<Project>> GetProject(int id)
        {

            var project = await _context.Projects.Include(M => M.Meetings)
                .FirstOrDefaultAsync(x => x.ProjectID == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }


        // GET: api/Projects/GetProjectByTitle/Facebook
        // This API returns Project with specific {title} including it's Meetings
        [Route("api/Projects/GetProjectByTitle")]
        [HttpGet("GetProjectByTitle/{title}")]
        [EnableCors]
        public async Task<ActionResult<Project>> GetProjectByTitle(string title)
        {
            var project = await _context.Projects.
                    Include(m => m.Meetings).
                    FirstOrDefaultAsync(x => x.ProjectTitle == title);

            if (project == null)
                return NotFound();

            return project;

        }

        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/Projects/UpdateProject/5
        // This API updates data related to Project with {id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Projects/UpdateProject")]
        [HttpPut("UpdateProject/{id}")]
        [EnableCors]
        public async Task<ActionResult<Project>> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectID)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updatedProject = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);
            return updatedProject;

        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/Projects/PostProject
        // This API creates a new Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Projects/PostProject")]
        [HttpPost("PostProject")]
        [EnableCors]
        public async Task<ActionResult<Project>> PostProject([FromBody] AddProjectModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == model.UserID);
            if(user == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectTitle == model.Project.ProjectTitle);
            if (project != null)
            {
                return BadRequest();
            }
            try
            {
                model.Project.User = user;
                _context.Projects.Add(model.Project);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }

            return CreatedAtAction("GetProject", new { id = model.Project.ProjectID }, model.Project);
        }

        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/Projects/DeleteProject/1
        // This API deletes a specific Project with {id}
        [Route("api/Projects/DeleteProject")]
        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }

        [Route("api/Projects/DetectConflicts")]
        [HttpGet("DetectConflicts/{id}")]
        [EnableCors]
        public async Task<ActionResult<Project>> DetectConflicts(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Meetings)
                .ThenInclude(m => m.Services)
                .ThenInclude(s => s.ServiceDetails).FirstOrDefaultAsync(p => p.ProjectID == id);
            if (project == null)
            {
                return BadRequest();
            }
            try
            {

                return project;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }

        }
    }
}
