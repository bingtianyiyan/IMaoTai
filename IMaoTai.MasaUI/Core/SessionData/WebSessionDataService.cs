using IMaoTai.Core.Domain;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.MasaUI.Core.SessionData
{
    /// <summary>
    /// web应用程序session
    /// </summary>
    public class WebSessionDataService : ISessionDataService
    {
        private readonly ProtectedSessionStorage _protectedSessionStore;

        public WebSessionDataService(ProtectedSessionStorage protectedSessionStorage)
        {
                _protectedSessionStore = protectedSessionStorage;
        }
        public async Task<UserLogin?> GetSession()
        {
            var userSessionStorageResult = await _protectedSessionStore.GetAsync<UserLogin>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            return userSession;
        }

        public async Task<bool> RemoveSession()
        {
            await _protectedSessionStore.DeleteAsync("UserSession");
            return true;
        }

        public async Task<bool> SetSession(UserLogin userLogin)
        {
            await _protectedSessionStore.SetAsync("UserSession",userLogin);
            return true;
        }
    }
}
