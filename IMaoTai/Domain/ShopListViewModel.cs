using System.Collections.ObjectModel;
using System.Windows.Input;
using IMaoTai.Entity;
using IMaoTai.Repository;

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

        #endregion
    }

    public class ShopListCache
    {
        public static List<ShopEntity> StoreList { get; set; } = new List<ShopEntity>();
    }
}
