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

    public class ServiceDetailsController : ControllerBase
    {
        private readonly BAIA_DB_Context _context;

        public ServiceDetailsController(BAIA_DB_Context context)
        {
            _context = context;
        }

        // GET: api/ServiceDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDetail>>> GetServiceDetails()
        {
            return await _context.ServiceDetails.ToListAsync();
        }

        // GET: api/ServiceDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDetail>> GetServiceDetail(int id)
        {
            var serviceDetail = await _context.ServiceDetails.FindAsync(id);

            if (serviceDetail == null)
            {
                return NotFound();
            }

            return serviceDetail;
        }

        // PUT: api/ServiceDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceDetail(int id, ServiceDetail serviceDetail)
        {
            if (id != serviceDetail.ServiceDetailID)
            {
                return BadRequest();
            }

            _context.Entry(serviceDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceDetailExists(id))
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

        // POST: api/ServiceDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors]
        public async Task<ActionResult<ServiceDetail>> PostServiceDetail(ServiceDetail serviceDetail)
        {
            _context.ServiceDetails.Add(serviceDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceDetail", new { id = serviceDetail.ServiceDetailID }, serviceDetail);
        }

        // DELETE: api/ServiceDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceDetail(int id)
        {
            var serviceDetail = await _context.ServiceDetails.FindAsync(id);
            if (serviceDetail == null)
            {
                return NotFound();
            }

            _context.ServiceDetails.Remove(serviceDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceDetailExists(int id)
        {
            return _context.ServiceDetails.Any(e => e.ServiceDetailID == id);
        }
    }
}
