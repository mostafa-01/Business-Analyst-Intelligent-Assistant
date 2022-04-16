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

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        //Get: api/Projects/GetMeetingTitles/5
        [Route("api/Projects/GetMeetingTitles")]
        [HttpGet("GetMeetingTitles/{id}")]
        public async Task<ActionResult<List<string>>> GetMeetingTitles(int id)
        {
            var project = new Project();
            project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);
            if (project == null)
                return NoContent();
            else
            {
                try {
                    
                    List<string> MeetingTitles = new List<string>();
                    var meetings = project.Meetings.ToList();
                    foreach (Meeting meeting in meetings)
                    {
                         MeetingTitles.Add(meeting.MeetingTitle);
                    }
                    return MeetingTitles;

                }catch(Exception e)
                {
                    Console.Out.WriteLine(e.ToString());
                    return StatusCode(500);
                }
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<Project>> PostProject([FromBody] AddProjectModel model)
        {
            if(_context.Projects.FirstOrDefault(x => x.ProjectTitle == model.project.ProjectTitle) != null)
            {
                return BadRequest();
            }
            try
            {
                model.project.User = _context.Users.FirstOrDefault(x => x.UserID == model.UserID);
                _context.Projects.Add(model.project);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }

            return CreatedAtAction("GetProject", new { id = model.project.ProjectID }, model.project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
