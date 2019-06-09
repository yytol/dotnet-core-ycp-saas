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
    public class AosAppsController : ApiControllerBase {

        public const string Platform_Name = "Aos";
        public const string Table_Name = "AosApps";

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    string aid = "" + Request.Form["aid"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[{Table_Name}]&[AuthID=='{aid}']+[Index]}}");
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


                    try {
                        if (id > 0) {
                            ui.EditSave(Platform_Name, Table_Name);
                        } else {
                            ui.AddSave(Platform_Name, Table_Name);
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

        [HttpPost("MoveUp")]
        public string MoveUp() {
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
                    long authId = form["AuthID"].ToLong();


                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        //获取当前项目的排序信息
                        var row = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}'&&ID=='{id}']}}");
                        if (row.IsEmpty) return Fail("未找到应用信息");

                        long idx = row["Index"].ToLong();
                        long idxMax = 0;

                        if (idx <= 0) {
                            var rowMax = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}']-[Index]}}");
                            if (!rowMax.IsEmpty) idxMax = rowMax["Index"].ToLong();
                            dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idxMax + 1}']&[ID=='{id}']}}");
                            JResponse.Message = "该应用尚未建立排序信息,创建排序信息成功,该应用将默认排在最后一位";
                            return Success();
                        }

                        //获取比当前项目排序数字低的排序最后的项目
                        long idxTarget = 0;
                        long idTarget = 0;
                        var rowTarget = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}'&&Index<'{idx}']-[Index]}}");
                        if (rowTarget.IsEmpty) return Fail("此项目已经在第一位，无法上移");
                        idTarget = rowTarget["ID"].ToLong();
                        idxTarget = rowTarget["Index"].ToLong();

                        //对调两个项目
                        dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idxTarget}']&[ID=='{id}']}}");
                        dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idx}']&[ID=='{idTarget}']}}");

                    }

                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("MoveDown")]
        public string MoveDown() {
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
                    long authId = form["AuthID"].ToLong();


                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {

                        //获取当前项目的排序信息
                        var row = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}'&&ID=='{id}']}}");
                        if (row.IsEmpty) return Fail("未找到应用信息");

                        long idx = row["Index"].ToLong();
                        long idxMax = 0;

                        if (idx <= 0) {
                            var rowMax = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}']-[Index]}}");
                            if (!rowMax.IsEmpty) idxMax = rowMax["Index"].ToLong();
                            dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idxMax + 1}']&[ID=='{id}']}}");
                            JResponse.Message = "该应用尚未建立排序信息,创建排序信息成功,该应用将默认排在最后一位";
                            return Success();
                        }

                        //获取比当前项目排序数字高的排序最靠前的项目
                        long idxTarget = 0;
                        long idTarget = 0;
                        var rowTarget = dbc.GetGdmlOne($"@{{$[{Table_Name}]&[AuthID=='{authId}'&&Index>'{idx}']+[Index]}}");
                        if (rowTarget.IsEmpty) return Fail("此项目已经在第一位，无法上移");
                        idTarget = rowTarget["ID"].ToLong();
                        idxTarget = rowTarget["Index"].ToLong();

                        //对调两个项目
                        dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idxTarget}']&[ID=='{id}']}}");
                        dbc.ExecGdml($"!{{$[{Table_Name}].[Index='{idx}']&[ID=='{idTarget}']}}");

                    }

                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

    }
}