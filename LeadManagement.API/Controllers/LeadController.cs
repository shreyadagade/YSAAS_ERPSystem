using LeadManagement.Application.DTOs.Lead;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Counsellor, Super User")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        // GET: api/Lead
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var leads = await _leadService.GetAllAsync();

            return Ok(leads);
        }

        // GET: api/Lead/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lead = await _leadService.GetByIdAsync(id);

            if (lead == null)
                return NotFound();

            return Ok(lead);
        }

        // POST: api/Lead
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] LeadDto lead)
        {
            var leadId =
                await _leadService.CreateAsync(lead);

            return Ok(new
            {
                message = "Lead Created Successfully",
                leadId = leadId
            });
        }

        // PUT: api/Lead/1
        [HttpPut("Update/{id:int}")]
     
        public async Task<IActionResult> Update(
    int id,
    [FromBody] LeadDto lead)
        {
            lead.LeadId = id;

            var result = await _leadService.UpdateAsync(lead);

            if (!result)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Lead not found."
                });
            }

            return Ok(new
            {
                statusCode = 200,
                message = "Lead Updated Successfully"
            });
        }

        // DELETE: api/Lead/1
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _leadService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Lead not found."
                });
            }

            return Ok(new
            {
                statusCode = 200,
                message = "Lead Deleted Successfully"
            });
        }

        // PUT: api/Lead/restore/1
        [HttpPut("restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _leadService.RestoreAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Lead not found or lead is already active."
                });
            }

            return Ok(new
            {
                statusCode = 200,
                message = "Lead Restored Successfully"
            });
        }
    }
}

