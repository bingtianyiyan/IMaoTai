using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using hygge_imaotai.Entity;
using MySql.Data.MySqlClient;

namespace hygge_imaotai.Repository
{
    /// <summary>
    /// 共用或基本的数据库操作
    /// </summary>
    internal class CommonRepository
    {
        /// <summary>
        /// 创建订单数据库
        /// </summary>
        public static void CreateDatabase()
        {
            if (App.DbType == FreeSql.DataType.Sqlite.ToString())
            {
                // 判断数据库文件是否存在
                if (File.Exists(App.OrderDatabasePath)) return;
                SQLiteConnection.CreateFile(App.OrderDatabasePath);
            }
            // 创建数据库连接
            IDbConnection sqlConnection = null;
            if (App.DbType == FreeSql.DataType.MySql.ToString())
            {
                sqlConnection = new MySqlConnection(App.OrderDatabaseConnectStr);
            }
            else
            {
                sqlConnection = new SQLiteConnection(App.OrderDatabaseConnectStr);
            }
            sqlConnection.Open();
            // 创建表结构
            var types = new[] { typeof(UserEntity),typeof(ShopEntity),typeof(LogEntity) };
            DB.SqlConn.CodeFirst.SyncStructure(types);
            
        }
    }
}
