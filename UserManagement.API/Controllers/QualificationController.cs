//using Microsoft.AspNetCore.Mvc;
//using UserManagement.Application.Interfaces;

//namespace UserManagement.API.Controllers
//{
//    [ApiController]
//    [Route("api/qualifications")]
//    public class QualificationController : ControllerBase
//    {
//        private readonly IQualificationService _qualificationService;

//        public QualificationController(IQualificationService qualificationService)
//        {
//            _qualificationService = qualificationService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _qualificationService.GetAllAsync();

//            return Ok(new
//            {
//                statusCode = StatusCodes.Status200OK,
//                message = "Qualifications retrieved successfully.",
//                data = result
//            });
//        }
//    }
//}