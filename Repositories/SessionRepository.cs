namespace Service.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly Dictionary<string, SessionData> _sessions;

        public SessionRepository()
        {
            _sessions = new Dictionary<string, SessionData>();
        }

        public SessionData? GetSessionData(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out SessionData? sessionData);
            return sessionData;
        }

        public SessionData? SaveSessionData(string sessionId, string deviceId)
        {
            SessionData? session = new SessionData() { DeviceId = deviceId, OTP = "000000" };
            session.AuthCode = session.GenerateAuthCode(sessionId);
            if (SaveSessionData(sessionId, session))
                return session;
            else
                return null;
        }

        public bool SaveSessionData(string sessionId, SessionData sessionData)
        {
            if (sessionData == null) return false;

            if (_sessions.ContainsKey(sessionId))
                _sessions.Remove(sessionId);

            _sessions.Add(sessionId, sessionData);

            return true;
        }

        public bool AuthoriseSession(string authCode, string x)
        {
            string[] pieces = authCode.Split('_');
            if (pieces.Length < 2)
                return false;

            long ticks = long.Parse(x);
            DateTime generated = new DateTime(ticks);
            DateTime limit = DateTime.UtcNow.AddSeconds(-30);
            if (generated < limit)
                return false;

            _sessions.TryGetValue(pieces[0], out SessionData sessionData);
            bool result = (sessionData != null && sessionData.OTP == pieces[1]);
            if (result)
            {
                sessionData.Authorised = true;
                SaveSessionData(pieces[0], sessionData);
            }
            return result;
        }
    }
}
