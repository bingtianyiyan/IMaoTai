using IMaoTai.Domain;
using IMaoTai.Entity;
using IMaoTai.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.Service
{
    public class UserService : IUserService
    {
        public UserListModel GetUserList(UserManageViewModel userListViewModel)
        {
            var result = new UserListModel();
            DB.SqlConn.Select<UserEntity>()
                .WhereIf(!string.IsNullOrEmpty(userListViewModel.Phone),
                    i => i.Mobile.Contains(userListViewModel.Phone))
                .WhereIf(!string.IsNullOrEmpty(userListViewModel.UserId),
                    i => (i.UserId + "").Contains(userListViewModel.UserId))
                .WhereIf(!string.IsNullOrEmpty(userListViewModel.Province),
                    i => i.ProvinceName.Contains(userListViewModel.Province))
                .WhereIf(!string.IsNullOrEmpty(userListViewModel.City), i => i.CityName.Contains(userListViewModel.City))
                .Count(out var total)
                .Page(userListViewModel.Current, userListViewModel.PageSize)
                .ToList().ForEach(result.UserList.Add);

            // 分页数据
            var pageCount = total / 10 + 1;
            result.Total = total;
            result.PageCount = pageCount;
            result.Current = userListViewModel.Current;
            return result;
        }
    }
}