using IMaoTai.Domain;
using IMaoTai.Entity;
using IMaoTai.Jobs;
using IMaoTai.Repository;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System.Configuration;
using System.IO;
using System.Windows;
using Yitter.IdGenerator;

namespace IMaoTai
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CacheDir = "cache";

        // 内部使用缓存文件
        public readonly string _productListFile = Path.Combine(CacheDir, "productList.json");

        private readonly string _sessionIdFile = Path.Combine(CacheDir, "mtSessionId.txt");

        // 商店共用缓存文件
        public static string StoreListFile = Path.Combine(CacheDir, "storeList.json");

        //用户信息缓存
        public static string UserListFile = Path.Combine(CacheDir, "userList.json");

        //日志信息缓存
        public static string LogListFile = Path.Combine(CacheDir, "logList.json");

        /// <summary>
        /// 订单数据库表名
        /// </summary>
        public const string OrderDatabasePath = "cache/imaotai.db";

        /// <summary>
        /// 订单数据库连接字符串
        /// </summary>
        public static string OrderDatabaseConnectStr = ConfigurationManager.AppSettings["DefaultConnStr"] ?? "Data Source=cache/imaotai.db;";

        public static string DbType = ConfigurationManager.AppSettings["DbType"] ?? "Sqlite";

        //是否启用本地文件存储
        public static bool LoadFromFile = ConfigurationManager.AppSettings["LoadFromFile"].ToString() == "1";

        /// <summary>
        /// 获取Freesql数据库类型
        /// </summary>
        /// <returns></returns>
        public static FreeSql.DataType GetFreeSqlDataType()
        {
            if (DbType == FreeSql.DataType.MySql.ToString())
            {
                return FreeSql.DataType.MySql;
            }
            else if (DbType == FreeSql.DataType.Oracle.ToString())
            {
                return FreeSql.DataType.Oracle;
            }
            else if (DbType == FreeSql.DataType.SqlServer.ToString())
            {
                return FreeSql.DataType.SqlServer;
            }
            return FreeSql.DataType.Sqlite;
        }

        /// <summary>
        /// 茅台会话ID
        /// </summary>
        public static string MtSessionId = "";

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //雪花id
            var options = new IdGeneratorOptions(8);
            // 保存参数（务必调用，否则参数设置不生效）：
            YitIdHelper.SetIdGenerator(options);

            // 判断cache文件夹是否存在
            if (!Directory.Exists(CacheDir))
                Directory.CreateDirectory(CacheDir);

            // 判断productListFile是否存在,存在则读入缓存
            if (File.Exists(_productListFile))
            {
                AppointProjectViewModel.ProductList = GetListFromFile<ProductEntity>(_productListFile);
            }
            // 开始初始化数据库
            CommonRepository.CreateDatabase();
            // 读取会话ID
            if (File.Exists(_sessionIdFile))
                MtSessionId = File.ReadAllText(_sessionIdFile);

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

        public static void WriteCache(string filename, string content)
        {
            var path = Path.Combine(CacheDir, filename);
            File.WriteAllText(path, content);
        }

        public static List<T> GetListFromFile<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var result = JsonConvert.DeserializeObject<List<T>>(json);
                return result;
            }
            return new List<T>();
        }

        public static void WriteDataToCache<T>(string path, List<T> list)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(list));
        }

        public static void FileContentClear(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}