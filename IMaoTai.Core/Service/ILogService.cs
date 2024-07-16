using IMaoTai.Core.Domain;

namespace IMaoTai.Core.Service
{
    public interface ILogService
    {
        Task<LogListModel> GetList(LogListViewModel storeListViewModel);

        Task DeleteAll();
    }
}