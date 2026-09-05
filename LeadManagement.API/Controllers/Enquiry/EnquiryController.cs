using LeadManagement.Application.DTOs.Enquiry;
using LeadManagement.Application.Interfaces.Services.Enquiry;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;

        public EnquiryController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }

        // =====================================================
        // CREATE ENQUIRY
        // POST: api/Enquiry/Create
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] EnquiryDto enquiry)
        {
            var enquiryId = await _enquiryService.CreateAsync(enquiry);

            return Ok(new
            {
                message = "Enquiry Created Successfully",
                enquiryId = enquiryId
            });
        }

        // =====================================================
        // UPDATE ENQUIRY
        // PUT: api/Enquiry/Update
        // =====================================================

        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] EnquiryDto enquiry)
        {
            var result = await _enquiryService.UpdateAsync(enquiry);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Enquiry not found."
                });
            }

            return Ok(new
            {
                message = "Enquiry Updated Successfully"
            });
        }

        // =====================================================
        // GET ENQUIRY BY ID
        // GET: api/Enquiry/GetById/{enquiryId}
        // =====================================================

               [HttpGet("GetById/{enquiryId}")]
        public async Task<IActionResult> GetById(int enquiryId)
        {
            // Validate Enquiry ID
            if (enquiryId <= 0)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    message = "Invalid enquiry ID."
                });
            }

            var enquiry = await _enquiryService.GetByIdAsync(enquiryId);

            if (enquiry == null)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Enquiry not found."
                });
            }

            return Ok(enquiry);
        }

        // =====================================================
        // GET ALL ENQUIRIES
        // GET: api/Enquiry/GetAll
        // =====================================================

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var enquiries = await _enquiryService.GetAllAsync();

            return Ok(enquiries);
        }

        // =====================================================
        // DELETE ENQUIRY
        // DELETE: api/Enquiry/Delete/{enquiryId}
        // =====================================================

        [HttpDelete("Delete/{enquiryId}")]
        public async Task<IActionResult> Delete(int enquiryId)
        {
            var result = await _enquiryService.DeleteAsync(enquiryId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Enquiry not found."
                });
            }

            return Ok(new
            {
                message = "Enquiry Deleted Successfully"
            });
        }

        // =====================================================
        // RESTORE ENQUIRY
        // PUT: api/Enquiry/Restore/{enquiryId}
        // =====================================================

        [HttpPut("Restore/{enquiryId}")]
        public async Task<IActionResult> Restore(int enquiryId)
        {
            var result = await _enquiryService.RestoreAsync(enquiryId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Enquiry not found."
                });
            }

            return Ok(new
            {
                message = "Enquiry Restored Successfully"
            });
        }

        // =====================================================
        // GET CANDIDATES FOR DROPDOWN
        // GET: api/Enquiry/Candidates
        // =====================================================

        [HttpGet("Candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _enquiryService.GetCandidatesAsync();

            return Ok(candidates);
        }
    }
}