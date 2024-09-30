using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Repositories;

namespace Service.Pages.UI
{
    public class AuthModel : PageModel
    {
        private readonly ISessionRepository _sessionRepository;

        public AuthModel(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public bool Authorised { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            string? authCode = Request.Query["authCode"];
            string? x = Request.Query["x"];

            Authorised = false;
            if (authCode != null)
            {
                try
                {
                    Authorised = _sessionRepository.AuthoriseSession(authCode, x);
                }
                catch(SessionAlreadyApprovedException appEx)
                {
                    ErrorMessage = "Session Already Approved";
                }
                catch(SessionCodeExpiredException codeEx)
                {
                    ErrorMessage = "QR Code Has Expired";
                }
                catch (Exception ex)
                {
                    ErrorMessage = "General Error";
                }
            }
            else
            {
                ErrorMessage = "No Auth Code";
            }
        }
    }
}
