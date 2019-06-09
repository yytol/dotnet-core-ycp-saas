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
    public class AosDesktopAuthorizesController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosAppLimits";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    //string aid = "" + Request.Form["aid"];
                    string desktopId = "" + Request.Form["DesktopID"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[AosAuthorize]}}");
                        var jList = new List<dpz.Dynamic>();
                        JData.List = jList;
                        foreach (var row in list) {

                            var rowAosUserApps = dbc.GetGdmlOne($"@{{$[AosDesktopAuthorizes]&[AuthID=='{row["ID"]}'&&DesktopID=='{desktopId}']}}");

                            if (rowAosUserApps.IsEmpty) {
                                row["Check"] = "no";
                            } else {
                                if (rowAosUserApps["Compatibility"].ToInteger() > 0) {
                                    row["Check"] = "yes";
                                } else {
                                    row["Check"] = "no";
                                } 
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
                    string desktopId = "" + Request.Form["DesktopID"];

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        var row = dbc.GetGdmlOne($"@{{$[AosDesktopAuthorizes]&[AuthID=='{authId}'&&DesktopID=='{desktopId}']}}");

                        if (row.IsEmpty) {
                            dbc.ExecGdml($"+{{$[AosDesktopAuthorizes].[AuthID='{authId}'].[DesktopID='{desktopId}'].[Compatibility='1']}}");
                            JData.Check = "yes";
                        } else {
                            if (row["Compatibility"].ToInteger() > 0) {
                                dbc.ExecGdml($"!{{$[AosDesktopAuthorizes].[Compatibility='0']&[ID=='{row["ID"]}']}}");
                                JData.Check = "no";
                            } else {
                                dbc.ExecGdml($"!{{$[AosDesktopAuthorizes].[Compatibility='1']&[ID=='{row["ID"]}']}}");
                                JData.Check = "yes";
                            }
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