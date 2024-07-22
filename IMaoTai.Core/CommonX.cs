using Newtonsoft.Json;
using System.Configuration;

namespace IMaoTai.Core
{
    public class CommonX
    {
        public static string CacheDir = Path.Combine( AppContext.BaseDirectory,"cache");

        // 内部使用缓存文件
        public static string _productListFile = Path.Combine(CacheDir, "productList.json");

        public static string _sessionIdFile = Path.Combine(CacheDir, "mtSessionId.txt");

        // 商店共用缓存文件
        public static string StoreListFile = Path.Combine(CacheDir, "storeList.json");

        //用户信息缓存
        public static string UserListFile = Path.Combine(CacheDir, "userList.json");

        //日志信息缓存
        public static string LogListFile = Path.Combine(CacheDir, "logList.json");


        //登录用户账号缓存
        public static string LoginUserListFile = Path.Combine(CacheDir, "loginUserList.json");

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