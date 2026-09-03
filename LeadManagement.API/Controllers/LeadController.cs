using LeadManagement.Application.DTOs.Lead;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Counsellor, Super User")]
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

            var result =
                await _leadService.UpdateAsync(lead);

            return Ok(new
            {
                message = "Lead Updated Successfully"
            });
        }

        // DELETE: api/Lead/1
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _leadService.DeleteAsync(id);

            return Ok(new
            {
                message = "Lead Deleted Successfully"
            });
        }

        // PUT: api/Lead/restore/1
        [HttpPut("restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _leadService.RestoreAsync(id);

            return Ok(new
            {
                message = "Lead Restored Successfully"
            });
        }
    }
}

