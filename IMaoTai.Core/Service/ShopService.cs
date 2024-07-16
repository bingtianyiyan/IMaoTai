using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Helpers;
using IMaoTai.Core.Repository;

namespace IMaoTai.Core.Service
{
    public class ShopService : IShopService
    {
        public async Task<ShopListModel> GetShopList(ShopListViewModel storeListViewModel)
        {
            var result = new ShopListModel();
            List<ShopEntity> list = new List<ShopEntity>();
            long total = 0;
            ShopListCache.StoreList.Clear();
            if (CommonX.LoadFromFile)
            {
                //lod from file
                list = CommonX.GetListFromFile<ShopEntity>(CommonX.StoreListFile);
                total = list.Count;
                //过滤数据
                if (list.Any())
                {
                    list = list.WhereIf(!string.IsNullOrEmpty(storeListViewModel.ShopId),
                    i => i.ShopId.Contains(storeListViewModel.ShopId))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Province),
                    i => i.Province.Contains(storeListViewModel.Province))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.City),
                    i => i.City.Contains(storeListViewModel.City))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Area), i => i.Area.Contains(storeListViewModel.Area))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.CompanyName), i => i.CompanyName.Contains(storeListViewModel.CompanyName))
                    .OrderByDescending(x => x.ShopId)
                    .PageSkipAndTake(storeListViewModel.Current, storeListViewModel.PageSize)
                    .ToList();
                }
            }
            else
            {
                list = await DB.SqlConn.Select<ShopEntity>()
               .WhereIf(!string.IsNullOrEmpty(storeListViewModel.ShopId),
                   i => i.ShopId.Contains(storeListViewModel.ShopId))
               .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Province),
                   i => i.Province.Contains(storeListViewModel.Province))
               .WhereIf(!string.IsNullOrEmpty(storeListViewModel.City),
                   i => i.City.Contains(storeListViewModel.City))
               .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Area), i => i.Area.Contains(storeListViewModel.Area))
               .WhereIf(!string.IsNullOrEmpty(storeListViewModel.CompanyName), i => i.CompanyName.Contains(storeListViewModel.CompanyName))
               .Count(out total)
              .OrderByDescending(x => x.ShopId)
              .Page(storeListViewModel.Current, storeListViewModel.PageSize)
               .ToListAsync();
            }
            foreach (var item in list)
            {
                result.ShopList.Add(item);
            }

            // 分页数据
            var pageCount = total / 10 + 1;
            result.Total = total;
            result.PageCount = pageCount;
            result.Current = storeListViewModel.Current;
            return result;
        }

        public async Task RefreshShop()
        {
            // 判断CommonX.StoreListFile是否存在,存在则删除
            if (File.Exists(CommonX.StoreListFile))
            {
                File.Delete(CommonX.StoreListFile);
            }
            await IMTService.RefreshShop();
            await GetShopList(new ShopListViewModel());
        }
    }
}