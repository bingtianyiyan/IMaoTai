using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Helpers;
using IMaoTai.Core.Repository;
using System.Text.RegularExpressions;

namespace IMaoTai.Core.Service
{
    public class UserService : IUserService
    {
        public async Task<UserListModel> GetUserList(UserManageViewModel userListViewModel)
        {
            long total = 0;
            var result = new UserListModel();
            List<UserEntity> list = new List<UserEntity>();
            if (CommonX.LoadFromFile)
            {
                //lod from file
                list = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                total = list.Count;
                //过滤数据
                if (list.Any())
                {
                    list = list.WhereIf(!string.IsNullOrEmpty(userListViewModel.Phone),
                        i => i.Mobile.Contains(userListViewModel.Phone))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.UserId),
                        i => (i.UserId + "").Contains(userListViewModel.UserId))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.Province),
                        i => i.ProvinceName.Contains(userListViewModel.Province))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.City), i => i.CityName.Contains(userListViewModel.City))
                    .OrderByDescending(x => x.CreateTime)
                    .PageSkipAndTake(userListViewModel.Current, userListViewModel.PageSize)
                    .ToList();
                }
            }
            else
            {
                list = await DB.SqlConn.Select<UserEntity>()
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.Phone),
                        i => i.Mobile.Contains(userListViewModel.Phone))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.UserId),
                        i => (i.UserId + "").Contains(userListViewModel.UserId))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.Province),
                        i => i.ProvinceName.Contains(userListViewModel.Province))
                    .WhereIf(!string.IsNullOrEmpty(userListViewModel.City), i => i.CityName.Contains(userListViewModel.City))
                    .Count(out total)
                    .OrderByDescending(x => x.CreateTime)
                    .Page(userListViewModel.Current, userListViewModel.PageSize)
                    .ToListAsync();
            }
            foreach (var item in list)
            {
                item.IdCardName = "";
                item.IdCardNo = "";
                result.UserList.Add(item);
            }

            // 分页数据
            var pageCount = total / 10 + 1;
            result.Total = total;
            result.PageCount = pageCount;
            result.Current = userListViewModel.Current;
            return result;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> ModifyUser(UserEntity model)
        {
            var dataLogicalItem = DataLogical(model);
            if (!dataLogicalItem.Item1)
            {
                return dataLogicalItem;
            }
            List<UserEntity> list = new List<UserEntity>();
            UserEntity foundUserEntity = null;
            if (CommonX.LoadFromFile)
            {
                //lod from file
                list = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                if (list.Any())
                {
                    foundUserEntity = list.FirstOrDefault(x => x.Mobile == model.Mobile);
                }
            }
            else
            {
                foundUserEntity = await DB.SqlConn.Select<UserEntity>()
                    .Where(x => x.Mobile == model.Mobile).FirstAsync();
            }
            if (foundUserEntity != null && foundUserEntity.UserId > 0)
            {
                if (CommonX.LoadFromFile)
                {
                    list.Remove(foundUserEntity);
                    //更新文件
                    // List<UserEntity> listNew = new List<UserEntity>();
                    model.JsonResult = foundUserEntity.JsonResult;
                    model.CreateTime = foundUserEntity.CreateTime;
                    list.Add(model);
                    if (list.Count != 0)
                        CommonX.WriteDataToCache(CommonX.UserListFile, list);
                    return (true, "");
                }
                else
                {
                    // 此处执行更新操作o
                    // 更新寻找到的用户信息
                    var res = await DB.SqlConn.Update<UserEntity>()
                    .Set(i => i.UserId, model.UserId)
                    .Set(i => i.Token, model.Token)
                    .Set(i => i.ItemCode, model.ItemCode)
                    .Set(i => i.ProvinceName, model.ProvinceName)
                    .Set(i => i.CityName, model.CityName)
                    .Set(i => i.Lat, model.Lat)
                    .Set(i => i.Lng, model.Lng)
                    .Set(i => i.ShopType, model.ShopType)
                    .SetIf(model.IdCardAuth >0, i => i.IdCardAuth, model.IdCardAuth)
                    .Set(i => i.ExpireTime, model.ExpireTime)
                    .Where(i => i.Mobile == model.Mobile).ExecuteAffrowsAsync();

                    return (res > 0 ? true : false, res > 0 ? "" : "编辑失败");
                }
            }
            return (false, "手机号不存在");
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> InsertUser(UserEntity model)
        {
            var dataLogicalItem = DataLogical(model);
            if (!dataLogicalItem.Item1)
            {
                return dataLogicalItem;
            }
            if (CommonX.LoadFromFile)
            {
                //lod from file
                var list = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                if (list.Any())
                {
                    var foundUserEntity = list.FirstOrDefault(x => x.Mobile == model.Mobile);
                    if (foundUserEntity != null)
                    {
                        return (false, "用户已存在");
                    }

                    list.Add(model);
                    if (list.Count != 0)
                        CommonX.WriteDataToCache(CommonX.UserListFile, list);
                    return (true, "");
                }
            }
            else
            {
                var foundUserEntity = await DB.SqlConn.Select<UserEntity>()
                .Where(x => x.Mobile == model.Mobile).FirstAsync();
                if (foundUserEntity != null)
                {
                    return (false, "用户已存在");
                }
                var res = await DB.SqlConn.Insert(model).ExecuteAffrowsAsync();
                if (res > 0)
                {
                    return (true, "");
                }
            }
            return (false, "新增失败");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(UserEntity model)
        {
            if (CommonX.LoadFromFile)
            {
                //lod from file
                var list = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                if (list.Any())
                {
                    var foundUserEntity = list.FirstOrDefault(x => x.Mobile == model.Mobile);
                    if (foundUserEntity == null)
                    {
                        return true;
                    }

                    list.Remove(foundUserEntity);
                    if (list.Count != 0 && list.Count > 0)
                    {
                        CommonX.WriteDataToCache(CommonX.UserListFile, list);
                    }
                    else
                    {
                        CommonX.FileContentClear(CommonX.UserListFile, string.Empty);
                    }
                }
                return true;
            }
            else
            {
                var res = await DB.SqlConn.Delete<UserEntity>().Where(x => x.Mobile == model.Mobile).ExecuteAffrowsAsync();
                return res > 0;
            }
        }

        private (bool, string) DataLogical(UserEntity model)
        {
            // 判断经纬度是否符合规范
            bool latIsInteger = Regex.IsMatch(model.Lat, @"^\d+$");
            bool latIsFloat = Regex.IsMatch(model.Lat, @"^\d+(\.\d+)?$");
            if (!latIsFloat && !latIsInteger)
            {
                return (false, "纬度不符合规范");
            }

            bool lngIsInteger = Regex.IsMatch(model.Lng, @"^\d+$");
            bool lngIsFloat = Regex.IsMatch(model.Lng, @"^\d+(\.\d+)?$");
            if (!latIsFloat && !latIsInteger)
            {
                return (false, "经度不符合规范");
            }
            return (true, "");
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> RealNameAuth(UserEntity model)
        {
           var res = await  IMTService.RealNameAuth(model);
            if (res)
            {
                await ModifyUser(model);
            }
            return res;
        }
    }
}