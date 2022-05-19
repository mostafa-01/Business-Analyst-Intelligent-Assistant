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

        // GET: api/Service/1
        // This API returns Service with {id}
        [Route("api/Services/GetAllServices")]
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

        // UPDATE

        // PUT: api/Services/UpdateService/1
        // This API updates User's data related to Service with {id}
        // Must send User Object in Body

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/Users/UpdateService")]
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

            return await _context.Services.FirstOrDefaultAsync(x => x.ServiceID == id); ;
            
        }

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]

        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.ServiceID }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceID == id);
        }
    }
}
