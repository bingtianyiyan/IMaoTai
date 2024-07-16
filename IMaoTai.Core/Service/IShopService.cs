using IMaoTai.Core.Domain;

namespace IMaoTai.Core.Service
{
    public interface IShopService
    {
        Task<ShopListModel> GetShopList(ShopListViewModel storeListViewModel);

        Task RefreshShop();
    }
}