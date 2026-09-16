using LeadManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagement.API.Controllers
{
    [ApiController]
    [Route("api/lead-import")]
    public class LeadImportController : ControllerBase
    {
        private readonly ILeadImportService _leadImportService;

        public LeadImportController(
            ILeadImportService leadImportService)
        {
            _leadImportService =
                leadImportService;
        }

        [HttpPost("excel")]
             [Consumes("multipart/form-data")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> ImportExcel(
            IFormFile file)
        {
            if (file == null ||
                file.Length == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Please upload an Excel file."
                });
            }

            if (!Path.GetExtension(file.FileName)
                    .Equals(
                        ".xlsx",
                        StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message =
                        "Only .xlsx Excel files are allowed."
                });
            }

            try
            {
                await using var stream =
                    file.OpenReadStream();

                var result =
                    await _leadImportService
                        .ImportExcelAsync(
                            stream,
                            file.FileName);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes
                        .Status500InternalServerError,
                    new
                    {
                        message =
                            "An error occurred while importing the Excel file."
                    });
            }
        }
    }
}