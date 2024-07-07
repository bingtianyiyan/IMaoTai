using IMaoTai.Entity;
using System.Collections.ObjectModel;
using IMaoTai.Repository;
using System.Windows.Input;

namespace IMaoTai.Domain
{
    /// <summary>
    /// 日志用户控件的ViewModel
    /// </summary>
    public class LogListViewModel:ViewModelBase
    {

        #region Properties

        public string Mobile { get; set; }

        public string Status { get; set; }

        public string SearchContent { get; set; }

        public int Current { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        #endregion

    }

    public class LogListModel
    {
        public List<LogEntity> LogList { get; set; } = new List<LogEntity>();

        public long Total { get; set; }

        /// <summary>
        /// 有几页
        /// </summary>
        public long PageCount { get; set; }

        public int Current { get; set; }

        public int PageSize { get; set; }
    }
}
