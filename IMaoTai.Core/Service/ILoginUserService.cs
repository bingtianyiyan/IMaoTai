using IMaoTai.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.Core.Service
{
    public interface ILoginUserService
    {

        Task<bool> Login(UserLogin loginRequest, string returnUrl);
        Task<CurrentUser> CurrentUserInfo();
        Task<string> Logout();
    }
}
