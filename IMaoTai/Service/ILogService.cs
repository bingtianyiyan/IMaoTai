using IMaoTai.Domain;

namespace IMaoTai.Service
{
    public interface ILogService
    {
        Task<LogListModel> GetList(LogListViewModel storeListViewModel);

        Task DeleteAll();
    }
}