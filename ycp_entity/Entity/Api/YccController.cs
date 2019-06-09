using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dpz;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Entity.Api {
    [Route("Api/[controller]")]
    [ApiController]
    public class YccController : ApiControllerBase {

        //[HttpPost("GetUserInfo")]
        //public string GetUserInfo(string sid) {
        //    try {
        //        string ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
        //          .Select(p => p.GetIPProperties())
        //          .SelectMany(p => p.UnicastAddresses)
        //          .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
        //          .FirstOrDefault()?.Address.ToString();
        //        string coreUrl = site.Config.Orm.CoreUrl.Replace("${IP}", ip);
        //        if (sid == null) {
        //            sid = "" + Request.Form["sid"];
        //        }
        //        string url = coreUrl + "/Api/User/GetInfo";
        //        //JData.Url = url;
        //        string args = "sid=" + HttpUtility.UrlEncode(sid, System.Text.Encoding.UTF8);
        //        //JData.Args = args;
        //        string res = dpz.Net.Http.PostUTF8(url, args);
        //        using (dpz.Jsons.Jttp jttp = new dpz.Jsons.Jttp(res)) {
        //            switch (jttp.Header.Status) {
        //                case "0":
        //                    return Fail(jttp.Message);
        //                case "-1":
        //                    return Error(jttp.Header.Error.ToInteger(), jttp.Message);
        //                default:
        //                    JObject dt = jttp.Data;
        //                    foreach (var item in dt) {
        //                        JData[item.Key] = item.Value.ToString();
        //                    }
        //                    using (SiteSession redis = new SiteSession()) {
        //                        redis.CreateSessionId();
        //                        redis.YccSessionId = sid;
        //                        redis.YccUserId = jttp.Data.ID;
        //                        redis.YccUserName = jttp.Data.Name;
        //                        redis.YccUserNick = jttp.Data.Nick;
        //                        JResponse.Header.SessionID = redis.SessionID;
        //                    }
        //                    break;
        //            }
        //        }
        //        return Success();
        //    } catch (Exception ex) {
        //        return Fail(ex.Message);
        //    }

        //}

        private string GetGetUserInfoBySid(string sid, string str, string md5) {

            if (sid.IsNone()) return Fail("交互标识无效");
            if (str.IsNone()) return Fail("缺少身份授权所需的随机字符串");
            if (md5.IsNone()) return Fail("缺少身份授权所需的验证码");

            string ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
              .Select(p => p.GetIPProperties())
              .SelectMany(p => p.UnicastAddresses)
              .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
              .FirstOrDefault()?.Address.ToString();
            string coreUrl = site.Config.Orm.CoreUrl.Replace("${IP}", ip);

            string url = coreUrl + "/Api/User/GetInfo";
            //JData.Url = url;
            string args = $"?sid={HttpUtility.UrlEncode(sid, System.Text.Encoding.UTF8)}&str={HttpUtility.UrlEncode(str, System.Text.Encoding.UTF8)}&md5={HttpUtility.UrlEncode(md5, System.Text.Encoding.UTF8)}";
            //JData.Args = args;
            string res = dpz.Net.Http.GetUTF8(url + args);
            using (dpz.Jsons.Jttp jttp = new dpz.Jsons.Jttp(res)) {
                switch (jttp.Header.Status) {
                    case "0":
                        return Fail(jttp.Message);
                    case "-1":
                        return Error(jttp.Header.Error.ToInteger(), jttp.Message);
                    default:
                        JObject dt = jttp.Data;
                        foreach (var item in dt) {
                            JData[item.Key] = item.Value.ToString();
                        }
                        using (SiteSession session = new SiteSession()) {
                            session.CreateNewSessionId();
                            //JResponse.Data.CacheCount1 = Program.CacheManager.Keys.Count;
                            session.YccSessionId = sid;
                            session.YccUserId = jttp.Data.ID;
                            session.YccUserName = jttp.Data.Name;
                            session.YccUserNick = jttp.Data.Nick;
                            //JResponse.Data.CacheCount2 = Program.CacheManager.Keys.Count;
                            JResponse.Header.SessionID = session.SessionID;
                        }
                        break;
                }
            }
            return Success();
        }

        [HttpGet("GetUserInfo")]
        public string GetUserInfo() {
            //try {
            string sid = Request.Query["sid"];
            string str = Request.Query["str"];
            string md5 = Request.Query["md5"];
            return GetGetUserInfoBySid(sid, str, md5);
            //} catch (Exception ex) {
            //    return Fail(ex.Message);
            //}

        }

        [HttpPost("GetUserInfo")]
        public string PostUserInfo() {
            //string sid = Request.Form["sid"];
            string sid = Request.Form["sid"];
            string str = Request.Form["str"];
            string md5 = Request.Form["md5"];
            return GetGetUserInfoBySid(sid, str, md5);

        }

        [HttpGet("GetSessionInfo")]
        public string GetSessionInfo() {
            //try {
            dpz.Dynamic dyc = JResponse.Data;
            foreach (string sid in Program.CacheManager.Keys) {
                var ob = new dpz.Dynamic();
                dyc[sid] = ob;
                var cache = Program.CacheManager[sid];
                foreach (string key in cache.Keys) {
                    ob[key] = cache[key];
                }
            }
            //} catch (Exception ex) {
            //    return Fail(ex.Message);
            //}
            return Success();

        }

    }
}