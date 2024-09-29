using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Repositories;

namespace Service.Pages.UI
{
    public class IndexModel : PageModel
    {
        public string SessionId { get; set; } = string.Empty;
        private readonly ISessionRepository _sessionRepository;

        public IndexModel(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public void OnGet()
        {
            string? sessionId = Request.Query["sessionId"];
            if (sessionId != null)
            {
                this.SessionId = sessionId;
                SessionData? sessionData = _sessionRepository.SaveSessionData(sessionId, Guid.NewGuid().ToString());
            }
        }
    }
}
