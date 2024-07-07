using FreeSql.DataAnnotations;

namespace IMaoTai.Entity
{
    /// <summary>
    /// 日志的模型
    /// </summary>
    public class LogEntity
    {
        #region Properties

        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        public string Status { get; set; }

        public string MobilePhone { get; set; }

        [Column(DbType = "longtext")]
        public string Content { get; set; }

        [Column(DbType = "longtext")]
        public string Response { get; set; }

        public DateTime CreateTime { get; set; }

        #endregion Properties

        #region Construct

        /// <summary>
        /// No Parameter Construct
        /// </summary>
        public LogEntity()
        {
        }

        public LogEntity(int id, string status, string mobilePhone, string content, string response, DateTime createTime)
        {
            Id = id;
            Status = status;
            MobilePhone = mobilePhone;
            Content = content;
            Response = response;
            CreateTime = createTime;
        }

        #endregion Construct
    }
}