using System;
using System.Windows.Input;
using FreeSql.DataAnnotations;
using IMaoTai.Domain;
using IMaoTai.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IMaoTai.Entity
{
    public class UserEntity
    {
        #region Properties
        [Column(IsIgnore = true)]
        public bool IsSelected { get; set; }
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

        [Column(DbType ="longtext")]
        public string JsonResult { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpireTime { get; set; } = DateTime.Now.AddDays(30);

        #endregion

        #region Commond

        //public ICommand DeleteCommand => new AnotherCommandImplementation(DeleteItemFunc);
        //public ICommand ModifyCommand => new AnotherCommandImplementation(ModifyItemFunc);
        //public ICommand ReserveCommand => new AnotherCommandImplementation(ReserveCommandItemFunc);

        //private static void DeleteItemFunc(object? parameter)
        //{
        //    var userEntity = (parameter as UserEntity)!;
        //    DB.SqlConn.Delete<UserEntity>().Where(i => i.Mobile == userEntity.Mobile).ExecuteAffrows();
        //    UserManageViewModel.UserList.Remove((parameter as UserEntity)!);
        //}

        //private static void ModifyItemFunc(object? parameter)
        //{
        //    //var userEntity = parameter as UserEntity;
        //    //// 深拷贝一份userEntity
        //    //var view = new DirectAddAccountDialogUserControl(JsonConvert.DeserializeObject<UserEntity>(JsonConvert.SerializeObject(userEntity)), true);
        //    //DialogHost.Show(view, "RootDialog");
        //}

        //private static async void ReserveCommandItemFunc(object? parameter)
        //{
        //    //var userEntity = parameter as UserEntity;
        //    //if (string.IsNullOrEmpty(userEntity?.ItemCode))
        //    //{
        //    //    new MessageBoxCustom("预约商品码未填写", MessageType.Error, MessageButtons.Ok).ShowDialog();
        //    //}
        //    //else
        //    //{
        //    //    try
        //    //    {
        //    //        await IMTService.Reservation(userEntity);
        //    //        new MessageBoxCustom("手动发起预约成功,响应结果请查看日志", MessageType.Success, MessageButtons.Ok).ShowDialog();
        //    //    }
        //    //    catch (Exception e)
        //    //    {
        //    //        new MessageBoxCustom("预约请求失败,响应结果详细请查看日志", MessageType.Error, MessageButtons.Ok).ShowDialog();
        //    //    }
        //    //}
        //}
        #endregion

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

        #endregion

    }
}
