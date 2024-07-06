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
        (bool, string) ModifyUser(UserEntity model);
        (bool, string) InsertUser(UserEntity model);
        bool DeleteUser(UserEntity model);
    }
}
