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

    public class ServicesController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public ServicesController(BAIA_DB_Context context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------//

        // READ

        // GET: api/Services/GetAllServices
        // This API returns all Services in Database
        [Route("api/Services/GetAllServices")]
        [HttpGet("GetAllServices")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services/GetService/1
        // This API returns Service with {id}
        [Route("api/Services/GetService")]
        [HttpGet("GetService/{id}")]
        [EnableCors]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }
        
        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/Services/UpdateService/1
        // This API updates data related to Service with {id}
        // Must send Service Object in Body

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Services/UpdateService")]
        [HttpPut("UpdateService/{id}")]
        [EnableCors]
        public async Task<ActionResult<Service>> UpdateService(int id, Service service)
        {
            if (id != service.ServiceID)
            {
                return BadRequest();
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updatedService = await _context.Services.FirstOrDefaultAsync(x => x.ServiceID == id);
            return updatedService;
        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/Services/PostService
        // This API creates a new Service
        // Must send Service object and MeetingID in Body

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Services/PostService")]
        [HttpPost("PostService")]
        [EnableCors]
        public async Task<ActionResult<Service>> PostService([FromBody] AddServiceModel model)
        {

            /*if (_context.Services.FirstOrDefault(x => x.ServiceTitle == model.Service.ServiceTitle) != null)
            {
                return BadRequest();
            }
            else
            {
                model.Service.Meeting = _context.Meetings.FirstOrDefault(x => x.MeetingID == model.MeetingID);
                _context.Services.Add(model.Service);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetService", new { id = model.Service.ServiceID }, model.Service);
            }*/

            var meeting = _context.Meetings.Include(m => m.Services).FirstOrDefault(x => x.MeetingID == model.MeetingID);
            if (meeting == null)
            {
                return NotFound();
            }

            /*var services = meeting.Services.ToList();
            bool serviceAlreadyExist = false;
            foreach (Service s in services)
            {
                if (s.ServiceTitle == model.Service.ServiceTitle)
                {
                    serviceAlreadyExist = true;
                    break;
                }
            }
            if (serviceAlreadyExist == true)
            {
                return BadRequest();
            }*/
            try
            {
                model.Service.Meeting = meeting;
                _context.Services.Add(model.Service);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }

            return CreatedAtAction("GetService", new { id = model.Service.ServiceID }, model.Service);

        }

        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/Services/DeleteService/1
        // This API deletes a specific Service with {id}
        [Route("api/Services/DeleteService")]
        [HttpDelete("DeleteService/{id}")]
        [EnableCors]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceID == id);
        }
    }


}
