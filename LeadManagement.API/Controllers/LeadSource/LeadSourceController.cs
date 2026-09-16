using LeadManagement.Application.DTOs.LeadSource;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadSourceController : ControllerBase
    {
        private readonly ILeadSourceService _service;

        public LeadSourceController(ILeadSourceService service)
        {
            _service = service;
        }

        // =====================================================
        // CREATE
        // POST: api/LeadSource
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] LeadSourceDto source)
        {
            var sourceId = await _service.CreateAsync(source);

            return Ok(new
            {
                message = "Lead source created successfully.",
                sourceId = sourceId
            });
        }

        // =====================================================
        // UPDATE
        // PUT: api/LeadSource/{sourceId}
        // =====================================================

        [HttpPut("{sourceId}")]
        public async Task<IActionResult> Update(
            int sourceId,
            [FromBody] LeadSourceDto source)
        {
            source.SourceId = sourceId;

            var result = await _service.UpdateAsync(source);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead source not found."
                });
            }

            return Ok(new
            {
                message = "Lead source updated successfully."
            });
        }

        // =====================================================
        // DELETE
        // DELETE: api/LeadSource/{sourceId}
        // =====================================================

        [HttpDelete("{sourceId}")]
        public async Task<IActionResult> Delete(int sourceId)
        {
            var result = await _service.DeleteAsync(sourceId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead source not found."
                });
            }

            return Ok(new
            {
                message = "Lead source deleted successfully."
            });
        }

        // =====================================================
        // RESTORE
        // PUT: api/LeadSource/restore/{sourceId}
        // =====================================================

        [HttpPut("restore/{sourceId}")]
        public async Task<IActionResult> Restore(int sourceId)
        {
            var result = await _service.RestoreAsync(sourceId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Lead source not found or already active."
                });
            }

            return Ok(new
            {
                message = "Lead source restored successfully."
            });
        }

        // =====================================================
        // GET BY ID
        // GET: api/LeadSource/{sourceId}
        // =====================================================

        [HttpGet("{sourceId}")]
        public async Task<IActionResult> GetById(int sourceId)
        {
            var source = await _service.GetByIdAsync(sourceId);

            if (source == null)
            {
                return NotFound(new
                {
                    message = "Lead source not found."
                });
            }

            return Ok(source);
        }

        // =====================================================
        // GET ALL
        // GET: api/LeadSource
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sources = await _service.GetAllAsync();

            return Ok(sources);
        }
    }
}