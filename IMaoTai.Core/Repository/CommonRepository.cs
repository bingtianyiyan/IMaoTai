using IMaoTai.Core.Entity;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SQLite;

namespace IMaoTai.Core.Repository
{
    /// <summary>
    /// 共用或基本的数据库操作
    /// </summary>
    public class CommonRepository
    {
        /// <summary>
        /// 创建订单数据库
        /// </summary>
        public static void CreateDatabase()
        {
            if (CommonX.LoadFromFile) { return; }
            if (CommonX.DbType == FreeSql.DataType.Sqlite.ToString())
            {
                // 判断数据库文件是否存在
                if (File.Exists(CommonX.OrderDatabasePath)) return;
                SQLiteConnection.CreateFile(CommonX.OrderDatabasePath);
            }
            // 创建数据库连接
            IDbConnection sqlConnection = null;
            if (CommonX.DbType == FreeSql.DataType.MySql.ToString())
            {
                sqlConnection = new MySqlConnection(CommonX.OrderDatabaseConnectStr);
            }
            else
            {
                sqlConnection = new SQLiteConnection(CommonX.OrderDatabaseConnectStr);
            }
            sqlConnection.Open();
            // 创建表结构
            var types = new[] { typeof(UserEntity), typeof(ShopEntity), typeof(LogEntity) };
            DB.SqlConn.CodeFirst.SyncStructure(types);
        }
    }
}