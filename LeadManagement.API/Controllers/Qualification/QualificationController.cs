using LeadManagement.Application.DTOs.Qualification;
using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QualificationController : ControllerBase
    {
        private readonly IQualificationService _service;

        public QualificationController(
            IQualificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var qualifications = await _service.GetAllAsync();

            return Ok(qualifications);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var qualification =
                await _service.GetByIdAsync(id);

            if (qualification == null)
            {
                return NotFound(new
                {
                    message = "Qualification not found."
                });
            }

            return Ok(qualification);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] QualificationDto qualification)
        {
            var qualificationId =
                await _service.CreateAsync(qualification);

            return Ok(new
            {
                message = "Qualification Created Successfully",
                qualificationId = qualificationId
            });
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] QualificationDto qualification)
        {
            qualification.QualificationId = id;

            await _service.UpdateAsync(qualification);

            return Ok(new
            {
                message = "Qualification Updated Successfully"
            });
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                message = "Qualification Deleted Successfully"
            });
        }

        [HttpPut("Restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);

            return Ok(new
            {
                message = "Qualification Restored Successfully"
            });
        }
    }
}