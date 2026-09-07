using LeadManagement.Application.DTOs.LeadFollowup;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadFollowupController : ControllerBase
    {
        private readonly ILeadFollowupService _service;

        public LeadFollowupController(
            ILeadFollowupService service)
        {
            _service = service;
        }

        // POST: api/LeadFollowup
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] LeadFollowupDto followup)
        {
            var followupId =
                await _service.CreateAsync(followup);

            return Ok(new
            {
                message = "Lead follow-up created successfully.",
                followupId = followupId
            });
        }

        // PUT: api/LeadFollowup
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] LeadFollowupDto followup)
        {
            var result =
                await _service.UpdateAsync(followup);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead follow-up not found."
                });
            }

            return Ok(new
            {
                message = "Lead follow-up updated successfully."
            });
        }

        // DELETE: api/LeadFollowup/5
        [HttpDelete("Delete/{followupId}")]
        public async Task<IActionResult> Delete(
            int followupId)
        {
            var result =
                await _service.DeleteAsync(followupId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead follow-up not found."
                });
            }

            return Ok(new
            {
                message = "Lead follow-up deleted successfully."
            });
        }

        // PUT: api/LeadFollowup/restore/5
        [HttpPut("restore/{followupId}")]
        public async Task<IActionResult> Restore(
            int followupId)
        {
            var result =
                await _service.RestoreAsync(followupId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead follow-up not found or already active."
                });
            }

            return Ok(new
            {
                message = "Lead follow-up restored successfully."
            });
        }

        // GET: api/LeadFollowup/5
        [HttpGet("GetbyId/{followupId}")]
        public async Task<IActionResult> GetById(
            int followupId)
        {
            var followup =
                await _service.GetByIdAsync(followupId);

            if (followup == null)
            {
                return NotFound(new
                {
                    message = "Lead follow-up not found."
                });
            }

            return Ok(followup);
        }

        // GET: api/LeadFollowup
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var followups =
                await _service.GetAllAsync();

            return Ok(followups);
        }
    }
}