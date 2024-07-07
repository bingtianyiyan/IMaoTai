using IMaoTai.Domain;
using IMaoTai.Entity;

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