using System;
using FreeSql.DataAnnotations;
using IMaoTai.Domain;
using Newtonsoft.Json.Linq;

namespace IMaoTai.Entity
{
    /// <summary>
    /// 店铺的实体类
    /// </summary>
    public class ShopEntity
    {

        #region Construct

        public ShopEntity()
        {
        }


        public ShopEntity(string shopId, JObject jObject)
        {
            ShopId = shopId;
            Province =  jObject.GetValue("provinceName").Value<string>();
            City = jObject.GetValue("cityName").Value<string>();
            Area = jObject.GetValue("districtName").Value<string>();
            UnbrokenAddress = jObject.GetValue("fullAddress").Value<string>();
            Lat = jObject.GetValue("lat").Value<string>();
            Lng = jObject.GetValue("lng").Value<string>();
            Name = jObject.GetValue("name").Value<string>();
            CompanyName = jObject.GetValue("tenantName").Value<string>();
            CreatedAt = DateTime.Now;
        }

        public ShopEntity(string shopId, string province, string city, string area, string unbrokenAddress, string lat, string lng, string name, string companyName)
        {
            ShopId = shopId;
            Province = province;
            City = city;
            Area = area;
            UnbrokenAddress = unbrokenAddress;
            Lat = lat;
            Lng = lng;
            Name = name;
            CompanyName = companyName;
            CreatedAt = DateTime.Now;
        }

        #endregion

        #region Properties

        public string ShopId { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string UnbrokenAddress { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public DateTime CreatedAt { get; set; }

        [Column(IsIgnore = true)]
        public double Distance { get; set; }

        #endregion
    }
}
