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

        public void OnGet()
        {
            string? authCode = Request.Query["authCode"];
            string? x = Request.Query["x"];
            if (authCode != null)
            {
                Authorised = _sessionRepository.AuthoriseSession(authCode, x);
            }
        }
    }
}
