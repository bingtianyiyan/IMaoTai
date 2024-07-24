using IMaoTai.Core.Domain;

namespace IMaoTai.MasaUI.Core.SessionData
{
    public interface ISessionDataService
    {
        Task<UserLogin?> GetSession();

        Task<bool> SetSession(UserLogin userLogin);

        Task<bool> RemoveSession();
    }
}
