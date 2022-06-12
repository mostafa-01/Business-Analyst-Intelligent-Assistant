using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BAIA.Data;
using System.Text.Json;
using BAIA.Models;
using Microsoft.AspNetCore.Cors;
using RestSharp;
using Microsoft.CodeAnalysis;

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

        //-----------------------------------------------------------------------//

        // READ

        // GET: api/Meetings/GetAllMeetings
        // This API returns all Meetings in Database
        [Route("api/Meetings/GetAllMeetings")]
        [HttpGet("GetAllMeetings")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetAllMeetings()
        {
            return await _context.Meetings.ToListAsync();
        }

        // GET: api/Meetings/GetMeeting/1
        // This API gets data of a specific Meeting with {id}
        // It includes Services and Service Details
        [Route("api/Meetings/GetMeeting")]
        [HttpGet("GetMeeting/{id}")]
        [EnableCors]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings
                .Include(s => s.Services)
                .ThenInclude(w => w.ServiceDetails)
                .FirstOrDefaultAsync(x => x.MeetingID == id);
            if (meeting == null)
            {
                return BadRequest();
            }
            else
            {
                if (meeting.Services.Count == 0)
                {
                    await GenerateServices(id);
                }
            }
            
            return meeting;
        }

        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/Meetings/UpdateMeeting/5
        // This API updates data related to Meeting with {id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Meetings/UpdateMeeting")]
        [HttpPut("UpdateMeeting/{id}")]
        [EnableCors]
        public async Task<ActionResult<Meeting>> UpdateMeeting(int id, Meeting meeting)
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
            var updatedMeeting = await _context.Meetings.FirstOrDefaultAsync(x => x.MeetingID == id);
            return updatedMeeting;
        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/Meetings/PostMeeting
        // This API creates a new Meeting
        // AddMeetingModel is a class contains a Meeting object and ProjectId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Meetings/PostMeeting")]
        [HttpPost("PostMeeting")]
        [EnableCors]
        public async Task<ActionResult<Meeting>> PostMeeting([FromBody] AddMeetingModel model)
        {
            var project = await _context.Projects.Include(p => p.Meetings).FirstOrDefaultAsync(x => x.ProjectID == model.ProjectID);
            if (project == null)
            {
                return NotFound();
            }
            var meetings = project.Meetings.ToList();
            bool meetingAlreadyExist = false;
            foreach (Meeting m in meetings)
            {
                if (m.MeetingTitle == model.Meeting.MeetingTitle)
                {
                    meetingAlreadyExist = true;
                    break;
                }
            }
            if (meetingAlreadyExist == true)
            {
                return BadRequest();
            }
            try
            {
                model.Meeting.Project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectID == model.ProjectID);

                var client = new RestClient($"http://127.0.0.1:5000/");
                var request = new RestRequest("meetingscript", Method.Post);
                request.AddJsonBody(new
                {
                    filepath = model.Meeting.AudioReference,
                    projectTitle = model.Meeting.Project.ProjectTitle,
                    domain = model.Meeting.Project.Domain,
                    actors = model.Meeting.MeetingPersonnel,
                    meetingTitle = model.Meeting.MeetingTitle,
                    meetingID = model.Meeting.MeetingID
                });
                RestResponse response = await client.ExecuteAsync(request);
                if (response.Content == null)
                    return NoContent();
                model.Meeting.ASR_Text = response.Content;

                _context.Meetings.Add(model.Meeting);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }
            return CreatedAtAction("GetMeeting", new { id = model.Meeting.MeetingID }, model.Meeting);
        }

        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/Meetings/DeleteMeeting/1
        // This API deletes a specific Meeting with {id}
        [Route("api/Meetings/DeleteMeeting")]
        [HttpDelete("DeleteMeeting/{id}")]
        [EnableCors]
        public async Task<ActionResult<Meeting>> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.MeetingID == id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.MeetingID == id);
        }

        //Get: api/Projects/GenerateServices/5
        //[Route("api/Meetings/GenerateServices")]
        //[HttpGet("GenerateServices/{id}")]
        //[EnableCors]
        private async Task<IActionResult> GenerateServices(int id)
        {
            var meeting = await _context.Meetings.FirstOrDefaultAsync(x => x.MeetingID == id);
            if (meeting == null)
                return BadRequest();
            else
            {
                try
                {
                    var client = new RestClient($"http://127.0.0.1:5000/");
                    var request = new RestRequest("services", Method.Post);
                    request.AddJsonBody(new
                    {
                        meetingscript = meeting.ASR_Text,
                        actors = meeting.MeetingPersonnel,
                        meetingTitle = meeting.MeetingTitle,
                        //projectID = meeting.Project.ProjectID
                    });
                    request.AddHeader("content-type", "application/json");
                    RestResponse response = await client.ExecuteAsync(request);

                    if (response.Content == null)
                        return NoContent();

                    var content = response.Content;
                    var ServicesDic = JsonSerializer
                        .Deserialize<List<GenerateServiceModel>>(content);


                    foreach (var item in ServicesDic)
                    {
                        Service srvc = new Service
                        {
                            ServiceTitle = item.serviceTitle,
                            Meeting = meeting
                        };
                        _context.Services.Add(srvc);
                        await _context.SaveChangesAsync();
                        foreach (var srvcDetail in item.serviceDetails)
                        {
                            int tsNum = Int32.Parse(srvcDetail.Timestamp);
                            TimeSpan t = TimeSpan.FromSeconds(tsNum);
                            string ts = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                            t.Hours,
                                            t.Minutes,
                                            t.Seconds);
                            srvcDetail.Timestamp = ts;
                            srvcDetail.Service = srvc;
                            _context.ServiceDetails.Add(srvcDetail);
                        }
                        await _context.SaveChangesAsync();
                    }

                    return Ok();

                   
                }
                catch (Exception e)
                {
                    return Content(e.ToString() + StatusCode(500));
                }
            }
        }
    }
}
