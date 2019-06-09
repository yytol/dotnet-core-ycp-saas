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
    public class AosUserAuthorizeController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosAppLimits";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    //string aid = "" + Request.Form["aid"];
                    string uid = "" + Request.Form["uid"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[AosAuthorize]}}");
                        var jList = new List<dpz.Dynamic>();
                        JData.List = jList;
                        foreach (var row in list) {

                            var rowAosUserApps = dbc.GetGdmlOne($"@{{$[AosUserAuthorize]&[AuthID=='{row["ID"]}'&&UserID=='{uid}']}}");

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

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        var row = dbc.GetGdmlOne($"@{{$[AosUserAuthorize]&[AuthID=='{authId}'&&UserID=='{userId}']}}");

                        if (row.IsEmpty) {
                            dbc.ExecGdml($"+{{$[AosUserAuthorize].[AuthID='{authId}'].[UserID='{userId}']}}");
                            JData.Check = "yes";
                        } else {
                            dbc.ExecGdml($"-{{$[AosUserAuthorize]&[AuthID=='{authId}'&&UserID=='{userId}']}}");
                            JData.Check = "no";
                        }

                    }

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