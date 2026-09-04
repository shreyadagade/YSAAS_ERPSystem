using LeadManagement.Application.DTOs.EnquiryFollowup;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Counsellor, Super User")]

    public class EnquiryFollowupController : ControllerBase
    {
        private readonly IEnquiryFollowupService _followupService;

        public EnquiryFollowupController(
            IEnquiryFollowupService followupService)
        {
            _followupService = followupService;
        }

        // GET: api/EnquiryFollowup
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var followups = await _followupService.GetAllAsync();

            return Ok(followups);
        }

        // GET: api/EnquiryFollowup/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var followup = await _followupService.GetByIdAsync(id);

            if (followup == null)
                return NotFound();

            return Ok(followup);
        }

        // POST: api/EnquiryFollowup
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] EnquiryFollowupDto followup)
        {
            var followupId = await _followupService.CreateAsync(followup);

            return Ok(new
            {
                message = "Follow Up Created Successfully",
                followupId = followupId
            });
        }

        // PUT: api/EnquiryFollowup/1
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] EnquiryFollowupDto followup)
        {
            followup.FollowupId = id;

            await _followupService.UpdateAsync(followup);

            return Ok(new
            {
                message = "Follow Up Updated Successfully"
            });
        }

        // DELETE: api/EnquiryFollowup/1
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _followupService.DeleteAsync(id);

            return Ok(new
            {
                message = "Follow Up Deleted Successfully"
            });
        }

        // PUT: api/EnquiryFollowup/restore/1
        [HttpPut("restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _followupService.RestoreAsync(id);

            return Ok(new
            {
                message = "Follow Up Restored Successfully"
            });
        }
    }
}

