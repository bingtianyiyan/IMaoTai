using IMaoTai.Entity;

namespace IMaoTai.Domain
{
    /// <summary>
    /// 门店列表Page的ViewModel
    /// </summary>
    public class ShopListViewModel
    {
        #region Properties

        public string ShopId { get; set; }

        public string Province { get; set; }
        public string City { get; set; }

        public string Area { get; set; }

        public string CompanyName { get; set; }

        public int Current { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        #endregion Properties
    }

    public class ShopListModel
    {
        public List<ShopEntity> ShopList { get; set; } = new List<ShopEntity>();

        public long Total { get; set; }

        /// <summary>
        /// 有几页
        /// </summary>
        public long PageCount { get; set; }

        public int Current { get; set; }

        public int PageSize { get; set; }
    }

    public class ShopListCache
    {
        public static List<ShopEntity> StoreList { get; set; } = new List<ShopEntity>();
    }
}