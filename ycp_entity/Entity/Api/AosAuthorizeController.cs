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
    public class AosAuthorizeController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosAuthorize";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                //JResponse.Data.CacheCount = Program.CacheManager.Count;
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

        [HttpPost("GetAppList")]
        public string GetAppList() {
            try {
                string sid = "" + Request.Form["sid"];

                using (SiteSession session = new SiteSession(sid)) {

                    //JResponse.Data.YccUserId = session.YccUserId;
                    //JResponse.Data.YccUserId2 = session[site.Session.S_YccUserId];
                    long uid = session.YccUserId.ToLong();
                    if (uid <= 0) return Fail("请先登录用户");
                    string user = session.YccUserName;
                    if (user != "root") return Fail("用户权限不足");

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    string url = "" + Request.Form["url"];
                    string ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                        .Select(p => p.GetIPProperties())
                        .SelectMany(p => p.UnicastAddresses)
                        .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                        .FirstOrDefault()?.Address.ToString();
                    url = url.Replace("${IP}", ip);

                    string appXmlString = dpz.Net.Http.GetUTF8(url);

                    using (dpz.Data.Xml xml = new dpz.Data.Xml(appXmlString)) {
                        var xmlEntity = xml["entity"];
                        var xmlApps = xmlEntity["apps"];
                        string appsSign = xmlApps.Attr["sign"];
                        if (appsSign != "") { appsSign += "."; }

                        //建立输出对象
                        var apps = new List<dpz.Dynamic>();
                        JData.List = apps;

                        foreach (var xmlApp in xmlApps.Nodes) {
                            if (xmlApp.Name == "app") {
                                string appSign = appsSign + xmlApp.Attr["sign"];
                                string appTitle = xmlApp.Attr["title"];
                                string appVer = xmlApp.Attr["version"];
                                string appDesc = xmlApp.Attr["description"];

                                var app = new dpz.Dynamic();
                                app["Name"] = appSign;
                                app["Path"] = xmlApp.Attr["path"];
                                app["Title"] = appTitle;
                                app["Description"] = appDesc;
                                app["Version"] = appVer;
                                app["SetupVersion"] = "";
                                apps.Add(app);

                                var rowVer = "";

                                using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                                    var rowApp = dbc.GetGdmlOne($"@{{$[AosApps]&[AuthID=='{aid}'&&Name=='{appSign}']}}");
                                    if (!rowApp.IsEmpty) { rowVer = rowApp["Version"]; }
                                }
                                app["SetupVersion"] = rowVer;
                                app["NeedUpdate"] = (rowVer == appVer ? "no" : "yes");
                            }
                        }
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("UpdateApp")]
        public string UpdateApp() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long uid = session.YccUserId.ToLong();
                    if (uid <= 0) return Fail("请先登录用户");
                    string user = session.YccUserName;
                    if (user != "root") return Fail("用户权限不足");

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    string url = "" + Request.Form["url"];
                    string sign = "" + Request.Form["sign"];

                    string ip = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                        .Select(p => p.GetIPProperties())
                        .SelectMany(p => p.UnicastAddresses)
                        .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                        .FirstOrDefault()?.Address.ToString();
                    url = url.Replace("${IP}", ip);

                    string appXmlString = dpz.Net.Http.GetUTF8(url);


                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        if (!dbc.CheckTable("AosApps")) {
                            return Fail($"请先安装AosApps表");
                        }

                        using (dpz.Data.Xml xml = new dpz.Data.Xml(appXmlString)) {
                            var xmlEntity = xml["entity"];
                            var xmlApps = xmlEntity["apps"];
                            string appsSign = xmlApps.Attr["sign"];
                            if (appsSign != "") { appsSign += "."; }
                            foreach (var xmlApp in xmlApps.Nodes) {
                                if (xmlApp.Name == "app") {
                                    string appSign = appsSign + xmlApp.Attr["sign"];

                                    if (appSign == sign) {
                                        string appTitle = xmlApp.Attr["title"];
                                        string appVer = xmlApp.Attr["version"];
                                        string appPath = xmlApp.Attr["path"];
                                        var rowVer = "";

                                        var rowApp = dbc.GetGdmlOne($"@{{$[AosApps]&[AuthID=='{aid}'&&Name=='{appSign}']}}");
                                        if (!rowApp.IsEmpty) { rowVer = rowApp["Version"]; }

                                        if (rowVer == "") {
                                            dbc.ExecGdml($"+{{$[AosApps].[AuthID='{aid}'].[Name='{appSign}'].[Text='{appTitle}'].[Path='{appPath}'].[Version='{appVer}'].[OnStore='0'].[IsDesktop='0'].[Description=''].[CatalogID='0']}}");
                                        } else if (appVer != rowVer) {
                                            dbc.ExecGdml($"!{{$[AosApps].[Text='{appTitle}'].[Path='{appPath}'].[Version='{appVer}']&[ID=='{rowApp["ID"]}']}}");
                                        }

                                        return Success();
                                    }

                                }
                            }


                        }

                    }
                }
                return Fail("未找到应用配置");
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

    }
}