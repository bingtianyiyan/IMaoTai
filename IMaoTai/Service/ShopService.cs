using IMaoTai.Domain;
using IMaoTai.Entity;
using IMaoTai.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace IMaoTai.Service
{
   public class ShopService:IShopService
    {

        public async Task<ShopListModel> GetShopList(ShopListViewModel storeListViewModel)
        {
            var result = new ShopListModel();
            ShopListCache.StoreList.Clear();
            var list = await DB.SqlConn.Select<ShopEntity>()
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.ShopId),
                    i => i.ShopId.Contains(storeListViewModel.ShopId))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Province),
                    i => i.Province.Contains(storeListViewModel.Province))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.City),
                    i => i.City.Contains(storeListViewModel.City))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Area), i => i.Area.Contains(storeListViewModel.Area))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.CompanyName), i => i.CompanyName.Contains(storeListViewModel.CompanyName))
                .Count(out var total)
               .Page(storeListViewModel.Current, storeListViewModel.PageSize)
                .ToListAsync();
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

        public  async Task RefreshShop()
        {
            // 判断App.StoreListFile是否存在,存在则删除
            if (File.Exists(App.StoreListFile))
            {
                File.Delete(App.StoreListFile);
            }
            await IMTService.RefreshShop();
            await GetShopList(new ShopListViewModel());
        }
    }
}
