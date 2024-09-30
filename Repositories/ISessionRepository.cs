using System.Xml.Linq;

namespace Service.Repositories
{
    public class SessionAlreadyApprovedException : Exception
    {
        public SessionAlreadyApprovedException() : base("Session already approved") { }
    }

    public class SessionCodeExpiredException : Exception
    {
        public SessionCodeExpiredException() : base("Session code expired") { }
    }

    public static class SessionDataHelpers
    {
        public static string GenerateAuthCode(this SessionData? sessionData, string sessionId)
            => $"{sessionId}_{sessionData.OTP}";
    }

    public class SessionData
    {
        public SessionData() { }

        public string DeviceId { get; set; }

        public string OTP { get; set; }

        public string AuthCode { get; set; }

        public bool Authorised { get; set; } = false;
    }

    public interface ISessionRepository
    {
        SessionData? GetSessionData(string sessionId);
        SessionData? SaveSessionData(string sessionId, string deviceId);
        bool SaveSessionData(string sessionId, SessionData sessionData);
        bool AuthoriseSession(string authCode, string x);
    }
}
