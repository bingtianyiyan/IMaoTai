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
    public class LogService:ILogService
    {
        public async Task<LogListModel> GetList(LogListViewModel storeListViewModel)
        {
            var result = new LogListModel();
            var list = await DB.SqlConn.Select<LogEntity>()
                    .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Mobile),
                    i => i.MobilePhone.Contains(storeListViewModel.Mobile))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.SearchContent),
                    i => i.Content.Contains(storeListViewModel.SearchContent))
                .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Status),
                    i => i.Status.Contains(storeListViewModel.Status))
                .Count(out var total)
                .OrderByDescending(x=> x.CreateTime)
               .Page(storeListViewModel.Current, storeListViewModel.PageSize)
                .ToListAsync();
            foreach (var item in list)
            {
                result.LogList.Add(item);
            }

            // 分页数据
            var pageCount = total / 10 + 1;
            result.Total = total;
            result.PageCount = pageCount;
            result.Current = storeListViewModel.Current;
            return result;
        }

        public async Task DeleteAll()
        {
           await DB.SqlConn.Delete<LogEntity>().ExecuteAffrowsAsync();
        }
    }
}
