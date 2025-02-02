﻿using IMaoTai.Core.Entity;

namespace IMaoTai.Core.Domain
{
    /// <summary>
    /// 用户管理 - 搜索的condition
    /// </summary>
    public class UserManageViewModel
    {
        #region Properties

        public string? Phone { get; set; }

        public string? UserId { get; set; }

        public string? Province { get; set; }

        public string? City { get; set; }

        public int Current { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        #endregion Properties
    }

    public class UserListModel
    {
        public List<UserEntity> UserList { get; set; } = new List<UserEntity>();

        public long Total { get; set; }

        /// <summary>
        /// 有几页
        /// </summary>
        public long PageCount { get; set; }

        public int Current { get; set; }

        public int PageSize { get; set; }
    }
}