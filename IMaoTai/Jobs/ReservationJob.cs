using NLog;
using Quartz;

namespace IMaoTai.Jobs
{
    public class ReservationJob : IJob
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion Fields

        public Task Execute(IJobExecutionContext context)
        {
            Logger.Info($"「批量预约开始」 {DateTime.Now}");
            return Task.Factory.StartNew(IMTService.ReservationBatch);
        }
    }

    public class RefreshJob : IJob
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion Fields

        public Task Execute(IJobExecutionContext context)
        {
            Logger.Info($"「刷新数据」开始刷新版本号，预约item，门店shop列表  {DateTime.Now}");
            return Task.Factory.StartNew(IMTService.RefreshAll);
        }
    }
}