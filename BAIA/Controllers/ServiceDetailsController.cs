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

        //-----------------------------------------------------------------------//

        // READ

        // GET: api/ServiceDetails/GetAllServiceDetails
        // This API returns all ServiceDetails in Database
        [Route("api/ServiceDetails/GetAllServiceDetails")]
        [HttpGet("GetAllServiceDetails")]
        [EnableCors]
        public async Task<ActionResult<IEnumerable<ServiceDetail>>> GetAllServiceDetails()
        {
            return await _context.ServiceDetails.ToListAsync();
        }

        // GET: api/ServiceDetails/GetServiceDetail/1
        // This API returns data of ServiceDetail with {id}
        [Route("api/ServiceDetails/GetServiceDetail")]
        [HttpGet("GetServiceDetail/{id}")]
        [EnableCors]
        public async Task<ActionResult<ServiceDetail>> GetServiceDetail(int id)
        {
            var serviceDetail = await _context.ServiceDetails.FindAsync(id);

            if (serviceDetail == null)
            {
                return NotFound();
            }

            return serviceDetail;
        }

        //-----------------------------------------------------------------------//

        // UPDATE

        // PUT: api/ServiceDetails/UpdateServiceDetail/1
        // This API updates data related to ServiceDetail with {id}
        // Must send ServiceDetail Object in Body

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/ServiceDetails/UpdateServiceDetail")]
        [HttpPut("UpdateServiceDetail/{id}")]
        [EnableCors]
        public async Task<ActionResult<ServiceDetail>> UpdateServiceDetail(int id, ServiceDetail serviceDetail)
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

            var updatedServiceDetail = await _context.ServiceDetails.FirstOrDefaultAsync(x => x.ServiceDetailID == id);
            return updatedServiceDetail;
        }

        //-----------------------------------------------------------------------//

        // CREATE

        // POST: api/ServiceDetails/PostServiceDetail
        // This API creates a new ServiceDetail
        // Must send ServiceDetail object and ServiceID in Body
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("api/ServiceDetails/PostServiceDetail")]
        [HttpPost("PostServiceDetail")]
        [EnableCors]
        public async Task<ActionResult<ServiceDetail>> PostServiceDetail([FromBody] AddServiceDetailModel model)
        {
            var service = _context.Services.FirstOrDefault(x => x.ServiceID == model.ServiceID);
            if (service == null)
            {
                return NotFound();
            }
            /*var serviceDetails = service.ServiceDetails.ToList();
            bool serviceDetailAlreadyExist = false;
            foreach (ServiceDetail sd in serviceDetails)
            {
                if (sd.ServiceDetailString == model.ServiceDetail.ServiceDetailString)
                {
                    serviceDetailAlreadyExist = true;
                    break;
                }
            }
            if (serviceDetailAlreadyExist == true)
            {
                return BadRequest();
            }*/
            try
            {
                model.ServiceDetail.Service = service;
                _context.ServiceDetails.Add(model.ServiceDetail);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
                return StatusCode(500);
            }


            return CreatedAtAction("GetServiceDetail", new { id = model.ServiceDetail.ServiceDetailID }, model.ServiceDetail);
        }


        //-----------------------------------------------------------------------//

        // DELETE

        // DELETE: api/ServiceDetails/5
        // This API deletes a specific ServiceDetail with {id}
        [Route("api/ServiceDetails/DeleteServiceDetail")]
        [HttpDelete("DeleteServiceDetail/{id}")]
        [EnableCors]
        public async Task<IActionResult> DeleteServiceDetail(int id)
        {
            var serviceDetail = await _context.ServiceDetails.FindAsync(id);
            if (serviceDetail == null)
            {
                return NotFound();
            }

            _context.ServiceDetails.Remove(serviceDetail);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ServiceDetailExists(int id)
        {
            return _context.ServiceDetails.Any(e => e.ServiceDetailID == id);
        }
    }
}
