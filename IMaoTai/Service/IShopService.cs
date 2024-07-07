using IMaoTai.Domain;

namespace IMaoTai.Service
{
    public interface IShopService
    {
        Task<ShopListModel> GetShopList(ShopListViewModel storeListViewModel);

        Task RefreshShop();
    }
}