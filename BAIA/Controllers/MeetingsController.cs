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

namespace BAIA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]

    public class MeetingsController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public MeetingsController(BAIA_DB_Context context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetMeetings()
        {
            return await _context.Meetings.ToListAsync();
        }

        [Route("api/Meetings/GetASR-Text")]
        [HttpGet("GetASR - Text /{id}")]
        public async Task<ActionResult<string>> GetASRText(int id)
        {
            var client = new RestClient($"http://127.0.0.1:5000/");
            var request = new RestRequest("meetingscript", Method.Post);
            request.AddJsonBody(new {filepath = _context.Meetings
                .FirstOrDefault(x=>x.MeetingID == id)
                .AudioReference});
            RestResponse response = await client.ExecuteAsync(request);
            return response.Content;
            //return StatusCode(500);
        }

        //Get: api/Projects/GetMeetingAsIs/5
        [Route("api/Meetings/GetMeetingAsIs")]
        [HttpGet("GetMeetingAsIs/{id}")]
        public async Task<ActionResult<List<string>>> GetMeetingAsIs(int id)
        {
            var meeting = new Meeting();
            meeting = await _context.Meetings.Include(p => p.Services).FirstOrDefaultAsync(x => x.MeetingID == id);
            if (meeting == null)
                return NoContent();
            else
            {
                try
                {

                    List<string> AsIs = new List<string>();
                    var Services = meeting.Services.ToList();
                    foreach (Service service in Services)
                    {
                        AsIs.Add(service.ServiceTitle + ":");
                        foreach(var DetailLine in _context.Services
                            .Include(z => z.ServiceDetails)
                            .FirstOrDefault(a => a.ServiceID == service.ServiceID)
                            .ServiceDetails
                            .ToList())
                        {
                            AsIs.Add(DetailLine.ServiceDetailString);
                        }
                    }
                    return AsIs;

                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.ToString());
                    return StatusCode(500);
                }
            }
        }

        // GET: api/Meetings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return meeting;
        }

        // PUT: api/Meetings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting(int id, Meeting meeting)
        {
            if (id != meeting.MeetingID)
            {
                return BadRequest();
            }

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
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

        // POST: api/Meetings
        // AddMeetingModel is a class contains a meeting object and project id
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<Meeting>> PostMeeting([FromBody] AddMeetingModel model)
        {
            try
            {
                model.meeting.Project = _context.Projects.FirstOrDefault(x => x.ProjectID == model.ProjectID);
                _context.Meetings.Add(model.meeting);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }
            return CreatedAtAction("GetMeeting", new { id = model.meeting.MeetingID }, model.meeting);
        }

        // DELETE: api/Meetings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.MeetingID == id);
        }
    }
}
