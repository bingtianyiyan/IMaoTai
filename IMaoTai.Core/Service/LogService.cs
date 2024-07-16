using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Helpers;
using IMaoTai.Core.Repository;

namespace IMaoTai.Core.Service
{
    public class LogService : ILogService
    {
        public async Task<LogListModel> GetList(LogListViewModel storeListViewModel)
        {
            var result = new LogListModel();
            List<LogEntity> list = new List<LogEntity>();
            long total = 0;
            if (CommonX.LoadFromFile)
            {
                //lod from file
                list = CommonX.GetListFromFile<LogEntity>(CommonX.LogListFile);
                total = list.Count;
                //过滤数据
                if (list.Any())
                {
                    list = list.WhereIf(!string.IsNullOrEmpty(storeListViewModel.Mobile),
                        i => i.MobilePhone.Contains(storeListViewModel.Mobile))
                    .WhereIf(!string.IsNullOrEmpty(storeListViewModel.SearchContent),
                        i => i.Content.Contains(storeListViewModel.SearchContent))
                    .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Status),
                        i => i.Status.Contains(storeListViewModel.Status))
                    .OrderByDescending(x => x.CreateTime)
                    .PageSkipAndTake(storeListViewModel.Current, storeListViewModel.PageSize)
                    .ToList();
                }
            }
            else
            {
                list = await DB.SqlConn.Select<LogEntity>()
                        .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Mobile),
                        i => i.MobilePhone.Contains(storeListViewModel.Mobile))
                    .WhereIf(!string.IsNullOrEmpty(storeListViewModel.SearchContent),
                        i => i.Content.Contains(storeListViewModel.SearchContent))
                    .WhereIf(!string.IsNullOrEmpty(storeListViewModel.Status),
                        i => i.Status.Contains(storeListViewModel.Status))
                    .Count(out total)
                    .OrderByDescending(x => x.CreateTime)
                   .Page(storeListViewModel.Current, storeListViewModel.PageSize)
                    .ToListAsync();
            }
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
            if (CommonX.LoadFromFile)
            {
                // 判断CommonX.StoreListFile是否存在,存在则删除
                if (System.IO.File.Exists(CommonX.LogListFile))
                {
                    System.IO.File.Delete(CommonX.LogListFile);
                }
            }
            else
            {
                await DB.SqlConn.Delete<LogEntity>().ExecuteAffrowsAsync();
            }
        }
    }
}