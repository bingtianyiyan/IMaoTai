using IMaoTai.Domain;
using IMaoTai.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.Service
{
    public interface IUserService
    {
        Task<UserListModel> GetUserList(UserManageViewModel userListViewModel);
        Task<(bool, string)> ModifyUser(UserEntity model);
        Task<(bool, string)> InsertUser(UserEntity model);
        Task<bool> DeleteUser(UserEntity model);
    }
}
