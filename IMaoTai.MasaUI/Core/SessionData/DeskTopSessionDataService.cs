using IMaoTai.Core;
using IMaoTai.Core.Domain;
using JsonFlatFileDataStore;

namespace IMaoTai.MasaUI.Core.SessionData
{
    /// <summary>
    /// 桌面应用程序session
    /// </summary>
    public class DeskTopSessionDataService : ISessionDataService
    {
        public async Task<UserLogin?> GetSession()
        {
            // Open database (create new if file doesn't exist)
            var store = new DataStore(CommonX.LoginCacheUserListFile);
            // Get employee collection
            var collection = store.GetCollection<UserLogin>();

            // Find item with name
            var userSession = collection
                                .AsQueryable()
                                .FirstOrDefault(x=> x.UserName == CommonX.LoginUserName);
            return await Task.FromResult( userSession);
        }

        public async Task<bool> RemoveSession()
        {
            // Open database (create new if file doesn't exist)
            var store = new DataStore(CommonX.LoginCacheUserListFile);
            // Get employee collection
            var collection = store.GetCollection<UserLogin>();

           var result = await collection.DeleteOneAsync(x => x.UserName == CommonX.LoginUserName);
            return result;
        }

        public async Task<bool> SetSession(UserLogin userLogin)
        {
            // Open database (create new if file doesn't exist)
            var store = new DataStore(CommonX.LoginCacheUserListFile);
            // Get employee collection
            var collection = store.GetCollection<UserLogin>();

            // Find item with name
            var userDynamic = collection
                                .AsQueryable()
                                .FirstOrDefault(p => p.UserName == userLogin.UserName);
            if (userDynamic == null)
            {
               return await collection.InsertOneAsync(userLogin);
            }
            else
            {
               return  await collection.UpdateOneAsync(p => p.UserName == userLogin.UserName, userLogin);
            }
        }
    }
}
