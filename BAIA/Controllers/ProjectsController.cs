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
            return await _context.Projects.Include(p => p.Meetings).ToListAsync();
        }

        // GET: api/Projects/GetMeetingTitles/4
        // This API returns all Meetings Titles in Database with Project {id}
        [Route("api/Projects/GetMeetingTitles")]
        [HttpGet("GetMeetingTitles/{id}")]
        [EnableCors]
        public async Task<ActionResult<List<string>>> GetMeetingTitles(int id)
        {
            var project = new Project();
            project = await _context.Projects.Include(p => p.Meetings).FirstOrDefaultAsync(x => x.ProjectID == id);
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

        // GET: api/Projects/5
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


        // GET: api/Projects/GetProjectByTitle/First Meeting
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
        // This API updates Project's data related to Project with {id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Projects/UpdateProject")]
        [HttpPut("UpdateProject/{id}")]
        [EnableCors]
        public async Task<IActionResult> PutProject(int id, Project project)
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

            return Ok();
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
            if (_context.Projects.FirstOrDefault(x => x.ProjectTitle == model.Project.ProjectTitle) != null)
            {
                return BadRequest();
            }
            try
            {
                model.Project.User = _context.Users.FirstOrDefault(x => x.UserID == model.UserID);
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
            var project = await _context.Projects.FindAsync(id);
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
    }
}
