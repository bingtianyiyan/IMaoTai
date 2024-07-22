using FreeSql.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace IMaoTai.Core.Entity
{
    public class UserEntity
    {
        #region Properties

        [Column(IsIgnore = true)]
        public bool IsSelected { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>

        [Column(IsIgnore = true)]
        public string IdCardName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        [Column(IsIgnore = true)]
        public string IdCardNo { get; set; }

        /// <summary>
        /// 是否已实名认证(1已实名,0 未实名)
        /// </summary>
        public int IdCardAuth {  get; set; }

        public long UserId { get; set; }

        public string Mobile { get; set; }

        public string PushPlusToken { get; set; }
        public string Token { get; set; }

        public string ItemCode { get; set; }

        public string ProvinceName { get; set; }

        public string CityName { get; set; }

        public string Address { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public int ShopType { get; set; } = 1;

        [Column(DbType = "longtext")]
        public string JsonResult { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpireTime { get; set; } = DateTime.Now.AddDays(30);

        #endregion Properties

        #region Construct Function

        public UserEntity()
        {
        }

        public UserEntity(string mobile, JObject jsonObject) : base()
        {
            var data = jsonObject["data"];
            this.UserId = data["userId"].Value<long>();
            this.Mobile = mobile;
            this.Token = data["token"].Value<string>();
            this.JsonResult = jsonObject.ToString();

            this.CreateTime = DateTime.Now;
            this.ExpireTime = DateTime.Now.AddDays(30);

            this.ItemCode = string.Empty;
            this.ProvinceName = string.Empty;
            this.CityName = string.Empty;
            this.Address = string.Empty;
            this.Lat = string.Empty;
            this.Lng = string.Empty;
            this.PushPlusToken = string.Empty;
        }

        #endregion Construct Function
    }
}