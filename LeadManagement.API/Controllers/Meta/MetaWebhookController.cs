//using Microsoft.AspNetCore.Mvc;

//namespace LeadManagement.API.Controllers
//{
//    [ApiController]
//    [Route("api/meta/webhook")]
//    public class MetaWebhookController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<MetaWebhookController> _logger;

//        public MetaWebhookController(
//            IConfiguration configuration,
//            ILogger<MetaWebhookController> logger)
//        {
//            _configuration = configuration;
//            _logger = logger;
//        }

//        // ============================================
//        // META WEBHOOK VERIFICATION
//        // ============================================
//        [HttpGet]
//        public IActionResult VerifyWebhook(
//            [FromQuery(Name = "hub.mode")] string? mode,
//            [FromQuery(Name = "hub.verify_token")] string? verifyToken,
//            [FromQuery(Name = "hub.challenge")] string? challenge)
//        {
//            var configuredToken =
//                _configuration["Meta:WebhookVerifyToken"];

//            if (mode == "subscribe" &&
//                verifyToken == configuredToken)
//            {
//                _logger.LogInformation(
//                    "Meta webhook verified successfully.");

//                return Content(
//                    challenge ?? string.Empty,
//                    "text/plain");
//            }

//            _logger.LogWarning(
//                "Meta webhook verification failed.");

//            return Forbid();
//        }


//        // ============================================
//        // META LEAD WEBHOOK
//        // ============================================
//        [HttpPost]
//        public async Task<IActionResult> ReceiveWebhook(
//            [FromBody] object payload)
//        {
//            _logger.LogInformation(
//                "Meta webhook received: {Payload}",
//                payload);

//            await Task.CompletedTask;

//            return Ok();
//        }
//    }
//}