using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IMaoTai.Core.Service
{
    public class LoginUserService:ILoginUserService
    {

        public async Task<bool> Login(UserLogin loginRequest, string returnUrl)
        {
            List<LoginUserEntity> list = await GetLoginUserList();
            if (list == null || list.Count == 0)
            {
                return false;
            }
            var user = list.FirstOrDefault(x=> x.UserName == loginRequest.UserName && x.Password == loginRequest.Password);
            if (user == null) return false;

            return true;
          
        }

        private async Task<List<LoginUserEntity>> GetLoginUserList()
        {
            List<LoginUserEntity> list = new List<LoginUserEntity>();
            if (CommonX.LoadFromFile)
            {
                //lod from file
                list = CommonX.GetListFromFile<LoginUserEntity>(CommonX.LoginUserListFile);
            }
            else
            {
                list = await DB.SqlConn.Select<LoginUserEntity>().ToListAsync();
            }
            return list;
        }

        //public async Task<CurrentUser> CurrentUserInfo()
        //{
        //    var currentUser = await GetCurrentUserInfoAsync(_httpContextAccessor.HttpContext.User);
        //    return await Task.FromResult(currentUser);
        //}

        public async Task<CurrentUser> GetCurrentUserInfoAsync(ClaimsPrincipal user)
        {
            var claimData = user.FindFirst(x => x.Type == ClaimTypes.Name);
            List<CaviarClaim> claims = null;
            if (claimData != null)//user.Identity.IsAuthenticated)
            {
                var applicationUser = await GetUserInfoAsync(claimData.Value);
                if (applicationUser == null)
                {
                    return new CurrentUser() { IsAuthenticated = false };
                }
                claims = new List<CaviarClaim>()
                {
                    new CaviarClaim("AccountName",applicationUser.UserName),
                };
                claims.AddRange(user.Claims.Select(u => new CaviarClaim(u)));
                var currentUser = new CurrentUser
                {
                    IsAuthenticated = true,//user.Identity.IsAuthenticated,
                    UserName = applicationUser.UserName,
                    Claims = claims
                };
                return await Task.FromResult(currentUser);
            }         
            else
            {
                var currentUser = new CurrentUser
                {
                    IsAuthenticated = user.Identity.IsAuthenticated,
                    UserName =  user.Identity.Name,
                    Claims = claims
                };
                return await Task.FromResult(currentUser);
            }
        }

        public Task<string> Logout()
        {
            return Task.FromResult("/login");
        }

        public async Task<LoginUserEntity> GetUserInfoAsync(string userName)
        {
            List<LoginUserEntity> list = await GetLoginUserList();
            if (list == null || list.Count == 0)
            {
                return null;
            }
            var user = list.FirstOrDefault(x => x.UserName == userName);
            return user;
        }

        public Task<CurrentUser> CurrentUserInfo()
        {
            throw new NotImplementedException();
        }
    }
}
