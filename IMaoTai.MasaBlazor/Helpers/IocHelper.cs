using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Jobs;
using IMaoTai.Core.Repository;
using IMaoTai.Core;
using IMaoTai.Core.Service;
using Masa.Blazor;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Impl;
using Quartz;
using Yitter.IdGenerator;

namespace IMaoTai.MasaBlazor.Web.Helpers;

public static class IocHelper
{
    public static IServiceCollection RegisterService(IServiceCollection _services)
    {
        _services.TryAddSingleton<IUserService, UserService>();
        _services.TryAddSingleton<IAppointProjectService, AppointProjectService>();
        _services.TryAddSingleton<IShopService, ShopService>();
        _services.TryAddSingleton<ILogService, LogService>();
        _services.TryAddScoped<I18n>();
        //_services.TryAddScoped<CookieStorage>();
        _services.AddHttpContextAccessor();
        return _services!;
    }

    public static async Task InitBusiness()
    {
        //雪花id
        var options = new IdGeneratorOptions(8);
        // 保存参数（务必调用，否则参数设置不生效）：
        YitIdHelper.SetIdGenerator(options);

        // 判断cache文件夹是否存在
        if (!Directory.Exists(CommonX.CacheDir))
            Directory.CreateDirectory(CommonX.CacheDir);

        // 判断productListFile是否存在,存在则读入缓存
        if (File.Exists(CommonX._productListFile))
        {
            AppointProjectViewModel.ProductList = CommonX.GetListFromFile<ProductEntity>(CommonX._productListFile);
        }
        // 开始初始化数据库
        CommonRepository.CreateDatabase();
        // 读取会话ID
        if (File.Exists(CommonX._sessionIdFile))
            CommonX.MtSessionId = File.ReadAllText(CommonX._sessionIdFile);

        // 创建任务调度器
        var stdSchedulerFactory = new StdSchedulerFactory();
        var scheduler = await stdSchedulerFactory.GetScheduler();
        await scheduler.Start();
        Console.WriteLine("任务调度器已启动");

        // 创建抢购预约的任务
        var jobDetail = JobBuilder.Create<ReservationJob>().Build();
        var trigger = TriggerBuilder.Create().WithCronSchedule("0 5 9 ? * * ").Build();
        // 创建刷新数据的任务
        var refreshJobDetail = JobBuilder.Create<RefreshJob>().Build();
        var refreshTrigger = TriggerBuilder.Create().WithCronSchedule("0 25,55 6,7,8 ? * * ").Build();
        // 添加调度
        await scheduler.ScheduleJob(jobDetail, trigger);
        await scheduler.ScheduleJob(refreshJobDetail, refreshTrigger);
    }
}