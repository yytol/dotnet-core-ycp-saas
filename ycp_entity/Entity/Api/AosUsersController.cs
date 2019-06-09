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
    public class AosUsersController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosUsers";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    string tabName = "" + Request.Form["table"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[{Table_Name}]}}");
                        var jList = new List<dpz.Dynamic>();
                        JData.List = jList;
                        foreach (var row in list) {
                            var jRow = new dpz.Dynamic();
                            jRow["DBStatus"] = "";
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

                    Dictionary<string, string> form = GetFormData();
                    //dpz.Data.Row form = GetFormData();
                    long id = form["ID"].ToLong();
                    //long id = ("" + Request.Form["ID"]).ToLong();
                    string cachePath = System.IO.Directory.GetCurrentDirectory() + site.Config.Orm.CachePath;

                    dpz.Mvc.UI.XmlUIForVue ui = new dpz.Mvc.UI.XmlUIForVue(site.Config.Database.Aos, form, site.Config.Orm.XmlUrl, cachePath);
                    dpz.Data.Row row = new dpz.Data.Row();

                    try {
                        if (id > 0) {
                            //修改
                            ui.EditSave(Platform_Name, Table_Name, row);
                        } else {
                            //添加
                            row["Code"] = Guid.NewGuid().ToString();
                            row["SecurityKey"] = Guid.NewGuid().ToString().Replace("-", "");
                            row["CreateUserID"] = "" + uid;
                            ui.AddSave(Platform_Name, Table_Name, row);
                        }
                    } catch (Exception ex) {
                        return Fail(ex.Message);
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

        [HttpPost("Repwd")]
        public string Repwd() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long uid = session.YccUserId.ToLong();
                    if (uid <= 0) return Fail("请先登录用户");
                    string user = session.YccUserName;
                    if (user != "root") return Fail("用户权限不足");

                    long id = ("" + Request.Form["ID"]).ToLong();
                    string name = "" + Request.Form["Name"];

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[AosUsers]&[ID=='{id}']}}");
                        if (!row.IsEmpty) {
                            dbc.ExecGdml($"!{{$[AosUsers].[Pwd='{site.Config.Security.GetEncryptionPasswordString(row["Name"], "000000")}']&[ID=='{id}']}}");
                        }
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }


    }
}