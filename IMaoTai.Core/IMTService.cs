﻿using Flurl.Http;
using IMaoTai.Core.Domain;
using IMaoTai.Core.Entity;
using IMaoTai.Core.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Yitter.IdGenerator;

namespace IMaoTai.Core
{
    public class IMTService
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion Fields

        private const string Salt = "2af72f100c356273d46284f6fd1dfc08";
        private static string _mtVersion = "";
        private const string AesKey = "qbhajinldepmucsonaaaccgypwuvcjaa";
        private const string AesIv = "2018534749963515";

        private static string Signature(string content, long timestamp)
        {
            var text = Salt + content + timestamp;
            using var md = MD5.Create();
            var hashBytes = md.ComputeHash(Encoding.UTF8.GetBytes(text));
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append($"{b:X2}");
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// 获得i茅台版本号
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetMtVersion()
        {
            if (!string.IsNullOrEmpty(_mtVersion)) return _mtVersion;
            var htmlSource = await "https://apps.apple.com/cn/app/i%E8%8C%85%E5%8F%B0/id1600482450"
                .GetStringAsync();
            var pattern = new Regex(@"new__latest__version\"">(.*?)<\/p>", RegexOptions.Singleline);
            var matcher = pattern.Match(htmlSource);
            var replace = "";
            if (!matcher.Success) return replace;
            var mtVersion = matcher.Groups[1].Value;
            replace = mtVersion.Replace("版本 ", "");
            _mtVersion = replace;
            return replace;
        }

        /// <summary>
        /// 向手机号发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SendCode(string phone)
        {
            var client = new HttpClient();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var values = new Dictionary<string, string>
        {
            { "mobile", phone },
            { "md5", Signature(phone,timestamp) },
            {"timestamp",timestamp+""}
        };
            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "iOS;16.3;Apple;?unrecognized?");
            client.DefaultRequestHeaders.Add("MT-Lat", "28.499562");
            client.DefaultRequestHeaders.Add("MT-K", "1675213490331");
            client.DefaultRequestHeaders.Add("MT-Lng", "102.182324");
            client.DefaultRequestHeaders.Add("Host", "app.moutai519.com.cn");
            client.DefaultRequestHeaders.Add("MT-User-Tag", "0");
            client.DefaultRequestHeaders.Add("MT-Network-Type", "WIFI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("MT-Team-ID", "");
            client.DefaultRequestHeaders.Add("MT-Info", "028e7f96f6369cafe1d105579c5b9377");
            client.DefaultRequestHeaders.Add("MT-Device-ID", "2F2075D0-B66C-4287-A903-DBFF6358342A");
            client.DefaultRequestHeaders.Add("MT-Bundle-ID", "com.moutai.mall");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-CN;q=1, zh-Hans-CN;q=0.9");
            client.DefaultRequestHeaders.Add("MT-Request-ID", "167560018873318465");
            client.DefaultRequestHeaders.Add("MT-APP-Version", await GetMtVersion());
            client.DefaultRequestHeaders.Add("MT-R", "clips_OlU6TmFRag5rCXwbNAQ/Tz1SKlN8THcecBp/HGhHdw==");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "93");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            var response = await client
                .PostAsync("https://app.moutai519.com.cn/xhr/front/user/register/vcode", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var code = (string)responseJson["code"];
            if (code == "2000") return true;
            throw new Exception(responseString);
        }

        /// <summary>
        /// 发送用户登录请求
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Login(string phone, string code)
        {
            var client = new HttpClient();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var values = new Dictionary<string, string>
            {
                { "mobile", phone },
                { "ydToken", "" },
                {"vCode",code},
                {"ydLogId",""},
                {"md5",Signature(phone + code + "" + "",timestamp)},
                {"timestamp",timestamp+""},
                {"MT-APP-Version",await GetMtVersion()}
            };
            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "iOS;16.3;Apple;?unrecognized?");
            client.DefaultRequestHeaders.Add("MT-Lat", "28.499562");
            client.DefaultRequestHeaders.Add("MT-K", "1675213490331");
            client.DefaultRequestHeaders.Add("MT-Lng", "102.182324");
            client.DefaultRequestHeaders.Add("Host", "app.moutai519.com.cn");
            client.DefaultRequestHeaders.Add("MT-User-Tag", "0");
            client.DefaultRequestHeaders.Add("MT-Network-Type", "WIFI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("MT-Team-ID", "");
            client.DefaultRequestHeaders.Add("MT-Info", "028e7f96f6369cafe1d105579c5b9377");
            client.DefaultRequestHeaders.Add("MT-Device-ID", "2F2075D0-B66C-4287-A903-DBFF6358342A");
            client.DefaultRequestHeaders.Add("MT-Bundle-ID", "com.moutai.mall");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-CN;q=1, zh-Hans-CN;q=0.9");
            client.DefaultRequestHeaders.Add("MT-Request-ID", "167560018873318465");
            client.DefaultRequestHeaders.Add("MT-APP-Version", await GetMtVersion());
            client.DefaultRequestHeaders.Add("MT-R", "clips_OlU6TmFRag5rCXwbNAQ/Tz1SKlN8THcecBp/HGhHdw==");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", "93");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            var response = await client
                .PostAsync("https://app.moutai519.com.cn/xhr/front/user/register/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var responseCode = (string)responseJson["code"];
            if (responseCode != "2000") throw new Exception(responseJson.TryGetValue("message", out var value) ? value.Value<string>() : responseString);
            // 存储一下数据

            if (CommonX.LoadFromFile)
            {
                //lod from file
                var list = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                if (list.Any())
                {
                    var foundUserEntity = list.FirstOrDefault(x => x.Mobile == phone);
                    if (foundUserEntity != null && foundUserEntity.UserId > 0)
                    {
                        list.Remove(foundUserEntity);
                        //更新token
                        var data = responseJson["data"];
                        string token = data["token"].Value<string>();
                        string jsonResult = responseJson.ToString();
                        foundUserEntity.JsonResult = jsonResult;
                        foundUserEntity.Token = token;
                        list.Add(foundUserEntity);
                        if (list.Count != 0)
                            CommonX.WriteDataToCache(CommonX.UserListFile, list);
                        return true;
                    }
                }
                list.Add(new UserEntity(phone, responseJson));
                CommonX.WriteDataToCache(CommonX.UserListFile, list);
                return true;
            }
            else
            {
                var foundUserEntity = await DB.SqlConn.Select<UserEntity>().Where(x => x.Mobile == phone).FirstAsync();
                if (foundUserEntity != null)
                {
                    //更新token
                    var data = responseJson["data"];
                    string token = data["token"].Value<string>();
                    string jsonResult = responseJson.ToString();

                    var res = await DB.SqlConn.Update<UserEntity>()
                        .Set(i => i.Token, token)
                        .Set(i => i.JsonResult, jsonResult)
                        .Where(i => i.Mobile == foundUserEntity.Mobile).ExecuteAffrowsAsync();
                    return res > 0;
                }
                else
                {
                    var res = await DB.SqlConn.Insert(new UserEntity(phone, responseJson)).ExecuteAffrowsAsync();
                    return res > 0;
                }
            }
        }

        /// <summary>
        /// 预约方法
        /// </summary>
        /// <param name="userEntity"></param>
        public static async Task Reservation(UserEntity userEntity)
        {
            var items = userEntity.ItemCode.Split("@");

            try
            {
                foreach (var item in items)
                {
                    var shopId = await ShopRepository.GetShopId(userEntity.ShopType, item, userEntity.ProvinceName, userEntity.CityName, userEntity.Lat, userEntity.Lng);
                    if (!string.IsNullOrEmpty(shopId))
                    {
                        await Reservation(userEntity, item, shopId);
                    }
                }
            }
            catch (Exception ex)
            {
                var logData = new LogEntity()
                {
                    CreateTime = DateTime.Now,
                    MobilePhone = userEntity.Mobile,
                    Content = $"[userId]:{userEntity.UserId}",
                    Response = ex.Message,
                    Status = "异常"
                };
                await LogWriteAsync(logData);
            }
        }

        public static async Task Reservation(UserEntity user, string itemId, string shopId)
        {
            var info = new Dictionary<string, object>
            {
                { "itemId", itemId },
                { "count", 1 }
            };

            var values = new Dictionary<string, object>
            {
                { "itemInfoList", new List<Dictionary<string, object>>() { info } },
                { "sessionId",await GetCurrentSessionId() },
                {"shopId",shopId},
                {"userId",user.UserId + ""}
            };
            values.Add("actParam", EncryptAES_CBC(JsonConvert.SerializeObject(values).Replace("\\\"", "\"")));
            var requestBody = JsonConvert.SerializeObject(values);

            var response = await "https://app.moutai519.com.cn/xhr/front/mall/reservation/add"
                .WithHeader("userId", user.UserId + "")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Connection", "keep-alive")
                .WithHeader("MT-Bundle-ID", "com.moutai.mall")
                .WithHeader("MT-R", "clips_OlU6TmFRag5rCXwbNAQ/Tz1SKlN8THcecBp/HGhHdw==")
                .WithHeader("Accept-Encoding", "gzip, deflate, br")
                .WithHeader("Accept-Language", "en-CN;q=1, zh-Hans-CN;q=0.9")
                .WithHeader("MT-Lat", user.Lat)
                .WithHeader("MT-K", "1675213490331")
                .WithHeader("MT-Lng", user.Lng)
                .WithHeader("Host", "app.moutai519.com.cn")
                .WithHeader("MT-User-Tag", "0")
                .WithHeader("Accept", "*/*")
                .WithHeader("MT-Network-Type", "WIFI")
                .WithHeader("MT-Token", user.Token)
                .WithHeader("MT-Team-ID", "")
                .WithHeader("MT-Info", "028e7f96f6369cafe1d105579c5b9377")
                .WithHeader("MT-Device-ID", "2F2075D0-B66C-4287-A903-DBFF6358342A")
                .WithHeader("MT-Request-ID", "167560018873318465")
                .WithHeader("MT-APP-Version", await GetMtVersion())
                .WithHeader("User-Agent", "iOS;16.3;Apple;?unrecognized?")
                .AllowAnyHttpStatus()
                .PostStringAsync(requestBody);
            if (response.StatusCode is 200 or 480)
            {
                var responseString = await response.GetStringAsync();
                var responseJson = JObject.Parse(responseString);
                var logData = new LogEntity()
                {
                    CreateTime = DateTime.Now,
                    MobilePhone = user.Mobile,
                    Content = $"[userId]:{user.UserId} [shopId]:{itemId}",
                    Response = responseString,
                    Status = responseJson["code"].Value<int>() == 2000 ? "预约成功" : "预约失败"
                };
                await LogWriteAsync(logData);
            }
            else
            {
                var logEntity = new LogEntity()
                {
                    CreateTime = DateTime.Now,
                    MobilePhone = user.Mobile,
                    Content = $"[userId]:{user.UserId} [shopId]:{itemId}",
                    Response = $"本次抢购会话已过期,请手动刷新一下商品列表和店铺列表后重试",
                    Status = "预约失败"
                };
                if (response.StatusCode != 404)
                {
                    logEntity.Response = await response.GetStringAsync();
                }
                await LogWriteAsync(logEntity);
            }
        }

        /// <summary>
        /// AES CBC加密函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string EncryptAES_CBC(string input)
        {
            var key = Encoding.UTF8.GetBytes(AesKey);
            var iv = Encoding.UTF8.GetBytes(AesIv);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor();

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            var base64String = Convert.ToBase64String(encryptedBytes);
            return base64String;
        }

        /// <summary>
        /// 获取当前会话的专属ID,并更新商品列表数据
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetCurrentSessionId()
        {
            var mtSessionId = CommonX.MtSessionId;
            if (!string.IsNullOrEmpty(mtSessionId)) return mtSessionId;

            var midNight = DateTime.Now.Date;
            var epochStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(8));
            var timeSpan = midNight.AddHours(-8) - epochStart;
            var milliseconds = (long)timeSpan.TotalMilliseconds;

            var responseStr = await ("https://static.moutai519.com.cn/mt-backend/xhr/front/mall/index/session/get/" +
                                    milliseconds)
                .GetStringAsync();
            var jObject = JObject.Parse(responseStr);
            if (jObject.GetValue("code").Value<int>() == 2000)
            {
                var dataJObject = jObject["data"];
                CommonX.MtSessionId = dataJObject["sessionId"].Value<string>();
                var itemList = (JArray)dataJObject["itemList"];
                AppointProjectViewModel.ProductList.Clear();
                foreach (var itemElement in itemList)
                {
                    AppointProjectViewModel.ProductList.Add(new ProductEntity(itemElement["itemCode"].Value<string>(),
                        itemElement["title"].Value<string>(),
                        itemElement["content"].Value<string>(),
                        itemElement["picture"].Value<string>(),
                        itemElement["pictureV2"].Value<string>(),
                        DateTime.Now));
                }
                CommonX.WriteCache("productList.json", JsonConvert.SerializeObject(AppointProjectViewModel.ProductList));
                CommonX.WriteCache("mtSessionId.txt", CommonX.MtSessionId);
            }

            return mtSessionId;
        }

        /// <summary>
        /// 批量预约
        /// </summary>
        public static void ReservationBatch()
        {
            List<UserEntity> users = new List<UserEntity>();
            if (CommonX.LoadFromFile)
            {
                var userList = CommonX.GetListFromFile<UserEntity>(CommonX.UserListFile);
                if (userList != null && userList.Count > 0)
                {
                    users = userList.Where(i =>
                        i.ExpireTime > DateTime.Now && !string.IsNullOrEmpty(i.Lat) && !string.IsNullOrEmpty(i.Lng) &&
                        !string.IsNullOrEmpty(i.ShopType + "") && !string.IsNullOrEmpty(i.ItemCode))
                    .ToList();
                }
            }
            else
            {
                users = DB.SqlConn.Select<UserEntity>().Where(i =>
                        i.ExpireTime > DateTime.Now && !string.IsNullOrEmpty(i.Lat) && !string.IsNullOrEmpty(i.Lng) &&
                        !string.IsNullOrEmpty(i.ShopType + "") && !string.IsNullOrEmpty(i.ItemCode))
                    .ToList();
            }
            foreach (var userEntity in users)
            {
                try
                {
                    Logger.Info($"「开始预约用户」{userEntity.Mobile}");
                    Reservation(userEntity);
                }
                catch (Exception e)
                {
                    Logger.Error($"用户{userEntity.Mobile}预约产生异常,错误原因:{e.Message}");
                    LogWrite(new LogEntity()
                    {
                        CreateTime = DateTime.Now,
                        MobilePhone = userEntity.Mobile,
                        Content = $"[userId]:{userEntity.UserId}",
                        Response = e.Message,
                        Status = "异常"
                    });
                }
            }
        }

        public static async void RefreshAll()
        {
            _mtVersion = string.Empty;
            await GetMtVersion();
            await RefreshShop();
            CommonX.MtSessionId = string.Empty;
            await GetCurrentSessionId();
        }

        public static async Task RefreshShop()
        {
            ShopListCache.StoreList.Clear();
            if (CommonX.LoadFromFile)
            {
                // 判断CommonX.StoreListFile是否存在,存在则删除
                if (System.IO.File.Exists(CommonX.StoreListFile))
                {
                    System.IO.File.Delete(CommonX.StoreListFile);
                }
            }
            else
            {
                await DB.SqlConn.Delete<ShopEntity>().ExecuteAffrowsAsync();
            }

            var responseStr = await "https://static.moutai519.com.cn/mt-backend/xhr/front/mall/resource/get"
                .GetStringAsync();
            var jObject = JObject.Parse(responseStr);
            var shopsUrl = jObject.GetValue("data").Value<JObject>().GetValue("mtshops_pc").Value<JObject>().GetValue("url").Value<string>();
            var shopInnerJson = await shopsUrl.GetStringAsync();

            var shopInnerJObject = JObject.Parse(shopInnerJson);
            List<ShopEntity> shopList = new List<ShopEntity>();
            foreach (var property in shopInnerJObject.Properties())
            {
                var shopId = property.Name;
                var nestedObject = (JObject)property.Value;
                shopList.Add(new ShopEntity(shopId, nestedObject));
            }
            if (shopList.Any())
            {
                if (CommonX.LoadFromFile)
                {
                    CommonX.WriteDataToCache(CommonX.StoreListFile, shopList);
                }
                else
                {
                    await DB.SqlConn.Insert(shopList).ExecuteAffrowsAsync();
                }
            }
            await Task.CompletedTask;
        }

        private static void LogWrite(LogEntity logData)
        {
            if (CommonX.LoadFromFile)
            {
                logData.Id = YitIdHelper.NextId();
                // 判断CommonX.StoreListFile是否存在,存在则删除
                var logHistoryList = CommonX.GetListFromFile<LogEntity>(CommonX.LogListFile);
                logHistoryList.Add(logData);
                CommonX.WriteDataToCache(CommonX.LogListFile, logHistoryList);
            }
            else
            {
                DB.SqlConn.Insert(logData).ExecuteAffrows();
            }
        }

        private static async Task LogWriteAsync(LogEntity logData)
        {
            if (CommonX.LoadFromFile)
            {
                logData.Id = YitIdHelper.NextId();
                // 判断CommonX.StoreListFile是否存在,存在则删除
                var logHistoryList = CommonX.GetListFromFile<LogEntity>(CommonX.LogListFile);
                logHistoryList.Add(logData);
                CommonX.WriteDataToCache(CommonX.LogListFile, logHistoryList);
            }
            else
            {
                await DB.SqlConn.Insert(logData).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> RealNameAuth(UserEntity user)
        {
            var client = new HttpClient();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var values = new Dictionary<string, string>
            {
                { "cardType", "0" },
                { "idCardName", user.IdCardName },
                {"idCardNo",user.IdCardNo}
            };
            var requestBody = JsonConvert.SerializeObject(values);
            try
            {
                var response = await "https://app.moutai519.com.cn/xhr/front/user/realNameAuth"
                .WithHeader("Content-Type", "application/json;charset=UTF-8")
                .WithHeader("Connection", "keep-alive")
                .WithHeader("MT-Bundle-ID", "com.moutai.mall")
                .WithHeader("MT-R", "clips_OlU6TmFRag5rCXwbNAQ/Tz1SKlN8THcecBp/HGhHdw==")
                .WithHeader("Accept-Encoding", "gzip, deflate, br")
                .WithHeader("Accept-Language", "en-CN;q=1, zh-Hans-CN;q=0.9")
                .WithHeader("MT-Lat", user.Lat)
                .WithHeader("MT-K", "1675213490331")
                .WithHeader("MT-Lng", user.Lng)
                .WithHeader("Host", "app.moutai519.com.cn")
                .WithHeader("MT-User-Tag", "0")
                .WithHeader("Accept", "*/*")
                .WithHeader("MT-Network-Type", "WIFI")
                .WithHeader("MT-Token", user.Token)
                .WithHeader("BS-DVID", "hWr62Q591-OLfaOq1ge1xkVW4OZfwq8jzbHpK2tx-B0Mhg0ZB8SVmhl0MzjFvXjHo7cnRUKQz5p9ChG1rFMxykw")
               .WithHeader("MT-DOUBLE", "0")
               .WithHeader("MT-SIM", "0")
              .WithHeader("MT-ACBE", "0")
               .WithHeader("MT-ACB", "0")
               .WithHeader("MT-ACBM", "0")
               .WithHeader("MT-Info", "028e7f96f6369cafe1d105579c5b9377")
                .WithHeader("MT-Team-ID", "")
                .WithHeader("MT-Info", "028e7f96f6369cafe1d105579c5b9377")
                .WithHeader("MT-Device-ID", "2F2075D0-B66C-4287-A903-DBFF6358342A")
                .WithHeader("MT-Request-ID", "167560018873318465")
                .WithHeader("MT-APP-Version", await GetMtVersion())
                .WithHeader("User-Agent", "iOS;16.3;Apple;?unrecognized?")
                .AllowAnyHttpStatus()
                .PostStringAsync(requestBody);
                var responseString = await response.GetStringAsync();
                var responseJson = JObject.Parse(responseString);
                var responseCode = (string)responseJson["code"];
                if (responseCode != "2000") return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"用户{user.Mobile}实名认证,错误原因:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}