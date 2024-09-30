using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using QRCoder;
using Service.Repositories;

namespace QRCodeAuth.Controllers
{
    [ApiController]
    [Route("Auth/QR")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ISessionRepository _sessionRepository;

        public AuthController(ILogger<AuthController> logger, ISessionRepository sessionRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        public IActionResult StartAuthByQRCode()
        {
            String sessionId = Guid.NewGuid().ToString();
            SessionData? sessionData = _sessionRepository.SaveSessionData(sessionId, Guid.NewGuid().ToString());
            if (sessionData != null)
                return new OkObjectResult(new { SessionId = sessionId, AuthCode = sessionData.AuthCode });
            else
                return new BadRequestResult();
        }

        [HttpPost]
        public IActionResult ValidateSessionByQRAuthCode(string authCode)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{sessionId}/image")]
        public IActionResult GetQRImage(string sessionId)
        {
            string uri = GenerateQRImageUri(this.HttpContext, sessionId);
            if (uri != string.Empty)
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.L))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeImage = qrCode.GetGraphic(20);
                    return File(qrCodeImage, "image/png");
                }
            }
            else
                return new BadRequestResult();
        }

        [HttpGet]
        [Route("{sessionId}/image/uri")]
        public IActionResult GetQRImageURL(string sessionId)
        {
            return new OkObjectResult(GenerateQRImageUri(this.HttpContext, sessionId));
        }

        private String GenerateQRImageUri(HttpContext context, string sessionId)
        {
            SessionData? session = _sessionRepository.GetSessionData(sessionId);
            if (session != null)
                return $"{context.Request.Scheme}://{context.Request.Host}/ui/auth?authcode={session.AuthCode}&x={DateTime.UtcNow.Ticks.ToString("x")}";
            else
                return string.Empty;
        }
    }
}
