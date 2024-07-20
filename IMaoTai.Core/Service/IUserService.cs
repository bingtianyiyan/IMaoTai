using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;

namespace IMaoTai.Core.Service
{
    public interface IUserService
    {
        Task<UserListModel> GetUserList(UserManageViewModel userListViewModel);

        Task<(bool, string)> ModifyUser(UserEntity model);

        Task<(bool, string)> InsertUser(UserEntity model);

        Task<bool> DeleteUser(UserEntity model);

        Task<bool> RealNameAuth(UserEntity model);
    }
}