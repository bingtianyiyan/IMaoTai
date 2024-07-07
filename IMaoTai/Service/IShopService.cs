using IMaoTai.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMaoTai.Service
{
    public interface IShopService
    {
        Task<ShopListModel> GetShopList(ShopListViewModel storeListViewModel);
        Task RefreshShop();
    }
}
