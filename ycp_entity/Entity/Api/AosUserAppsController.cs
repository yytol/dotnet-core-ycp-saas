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
    public class AosUserAppsController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosAppLimits";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    string aid = "" + Request.Form["aid"];
                    string uid = "" + Request.Form["uid"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[AosApps]&[AuthID=='{aid}']}}");
                        var jList = new List<dpz.Dynamic>();
                        JData.List = jList;
                        foreach (var row in list) {

                            var rowAosUserApps = dbc.GetGdmlOne($"@{{$[AosUserApps]&[AppID=='{row["ID"]}'&&AuthID=='{aid}'&&UserID=='{uid}']}}");

                            if (rowAosUserApps.IsEmpty) {
                                row["Check"] = "no";
                            } else {
                                row["Check"] = "yes";
                            }

                            var jRow = new dpz.Dynamic();
                            //jRow["DBStatus"] = "";
                            foreach (var item in row) {
                                jRow[item.Key] = item.Value;
                            }
                            jList.Add(jRow);
                        }
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("GetUserAuthList")]
        public string GetUserAuthList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    //string aid = "" + Request.Form["aid"];
                    string uid = "" + Request.Form["uid"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[AosAuthorize].[*]$[AosUserAuthorize]&[AosAuthorize.ID==AosUserAuthorize.AuthID&&AosUserAuthorize.UserID=='{uid}']}}");
                        var jList = new List<dpz.Dynamic>();
                        JData.List = jList;
                        foreach (var row in list) {
                            var jRow = new dpz.Dynamic();
                            //jRow["DBStatus"] = "";
                            foreach (var item in row) {
                                jRow[item.Key] = item.Value;
                            }
                            jList.Add(jRow);
                        }
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("Save")]
        public string Save() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long uid = session.YccUserId.ToLong();
                    if (uid <= 0) return Fail("请先登录用户");
                    string user = session.YccUserName;
                    if (user != "root") return Fail("用户权限不足");

                    string authId = "" + Request.Form["AuthID"];
                    string userId = "" + Request.Form["UserID"];
                    string appId = "" + Request.Form["AppID"];

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        var row = dbc.GetGdmlOne($"@{{$[AosUserApps]&[AppID=='{appId}'&&AuthID=='{authId}'&&UserID=='{userId}']}}");

                        if (row.IsEmpty) {
                            dbc.ExecGdml($"+{{$[AosUserApps].[AppID='{appId}'].[AuthID='{authId}'].[UserID='{userId}']}}");
                            JData.Check = "yes";
                        } else {
                            dbc.ExecGdml($"-{{$[AosUserApps]&[AppID=='{appId}'&&AuthID=='{authId}'&&UserID=='{userId}']}}");
                            JData.Check = "no";
                        }

                    }

                    //string cols = $".[Name='{Request.Form["Name"]}'].[DBType='{Request.Form["DBType"]}'].[DBSign='{Request.Form["DBSign"]}'].[DBIP='{Request.Form["DBIP"]}'].[DBPort='{Request.Form["DBPort"]}'].[DBUser='{Request.Form["DBUser"]}'].[DBPwd='{Request.Form["DBPwd"]}'].[UrlEntrance='{Request.Form["UrlEntrance"]}'].[ScriptEntrance='{Request.Form["ScriptEntrance"]}']";


                    //using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                    //    if (id > 0) {
                    //        dbc.ExecGdml($"!{{$[AosAuthorize]{cols}&[ID=='{id}']}}");
                    //    } else {
                    //        cols += $".[Code='{Guid.NewGuid().ToString()}'].[SecurityKey='{Guid.NewGuid().ToString().Replace("-", "")}']";
                    //        dbc.ExecGdml($"+{{$[AosAuthorize]{cols}}}");
                    //    }
                    //}
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("Delete")]
        public string Delete() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long uid = session.YccUserId.ToLong();
                    if (uid <= 0) return Fail("请先登录用户");
                    string user = session.YccUserName;
                    if (user != "root") return Fail("用户权限不足");

                    long id = ("" + Request.Form["ID"]).ToLong();

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        dbc.ExecGdml($"-{{$[{Table_Name}]&[ID=='{id}']}}");
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

    }
}