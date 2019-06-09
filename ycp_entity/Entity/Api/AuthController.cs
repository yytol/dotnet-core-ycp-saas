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
    public class AuthController : ApiControllerBase {

        [HttpPost("GetList")]
        public string GetList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {
                    string tabName = "" + Request.Form["table"];
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var list = dbc.GetGdmlList($"@{{$[AosAuthorize]}}");
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

                    long id = ("" + Request.Form["ID"]).ToLong();
                    string cols = $".[Name='{Request.Form["Name"]}'].[DBType='{Request.Form["DBType"]}'].[DBSign='{Request.Form["DBSign"]}'].[DBIP='{Request.Form["DBIP"]}'].[DBPort='{Request.Form["DBPort"]}'].[DBUser='{Request.Form["DBUser"]}'].[DBPwd='{Request.Form["DBPwd"]}'].[UrlEntrance='{Request.Form["UrlEntrance"]}'].[ScriptEntrance='{Request.Form["ScriptEntrance"]}']";


                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        if (id > 0) {
                            dbc.ExecGdml($"!{{$[AosAuthorize]{cols}&[ID=='{id}']}}");
                        } else {
                            cols += $".[Code='{Guid.NewGuid().ToString()}'].[SecurityKey='{Guid.NewGuid().ToString().Replace("-", "")}']";
                            dbc.ExecGdml($"+{{$[AosAuthorize]{cols}}}");
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
                        dbc.ExecGdml($"-{{$[AosAuthorize]&[ID=='{id}']}}");
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("CheckOrmDatebase")]
        public string CheckOrmDatebase() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    dpz.Database db;
                    string dbName = "";

                    if (aid <= 0) return Fail("未找到授权信息");

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[AosAuthorize]&[ID=='{aid}']}}");
                        if (row.IsEmpty) return Fail("授权信息无效");

                        dbName = "Aos_" + row["DBSign"];
                        if (row["DBIP"] != "") {
                            switch (row["DBType"].ToLower()) {
                                case "sqlserver":
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = row["DBIP"],
                                        Name = "master",
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                case "mysql":
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = row["DBIP"],
                                        Name = "mysql",
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        } else {
                            var dbAos = site.Config.Database.Aos;
                            switch (dbAos.Type) {
                                case DatabaseTypes.Microsoft_SQL_Server:
                                    var dbSql = dbAos as dpz.Gdbc.Databases.MicrosoftSqlServer;
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = dbSql.Address,
                                        Name = "master",
                                        Password = dbSql.Password,
                                        Port = dbSql.Port,
                                        User = dbSql.User
                                    };
                                    break;
                                case DatabaseTypes.MySQL:
                                    var dbMysql = dbAos as dpz.Gdbc.Databases.MySql;
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = dbMysql.Address,
                                        Name = "mysql",
                                        Password = dbMysql.Password,
                                        Port = dbMysql.Port,
                                        User = dbMysql.User
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        }

                    }

                    //try {
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(db)) {
                        if (dbc.CheckDatabase(dbName)) {
                            return Success();
                        } else {
                            return Fail();
                        }
                    }
                    //} catch (Exception ex) {
                    //    throw new Exception(db.ToString(), ex);
                    //}


                }
                //return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("CreateOrmDatebase")]
        public string CreateOrmDatebase() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    dpz.Database db;
                    string dbName = "";

                    if (aid <= 0) return Fail("未找到授权信息");

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[AosAuthorize]&[ID=='{aid}']}}");
                        if (row.IsEmpty) return Fail("授权信息无效");

                        dbName = "Aos_" + row["DBSign"];
                        if (row["DBIP"] != "") {
                            switch (row["DBType"].ToLower()) {
                                case "sqlserver":
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = row["DBIP"],
                                        Name = "master",
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                case "mysql":
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = row["DBIP"],
                                        Name = "mysql",
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        } else {
                            var dbAos = site.Config.Database.Aos;
                            switch (dbAos.Type) {
                                case DatabaseTypes.Microsoft_SQL_Server:
                                    var dbSql = dbAos as dpz.Gdbc.Databases.MicrosoftSqlServer;
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = dbSql.Address,
                                        Name = "master",
                                        Password = dbSql.Password,
                                        Port = dbSql.Port,
                                        User = dbSql.User
                                    };
                                    break;
                                case DatabaseTypes.MySQL:
                                    var dbMysql = dbAos as dpz.Gdbc.Databases.MySql;
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = dbMysql.Address,
                                        Name = "mysql",
                                        Password = dbMysql.Password,
                                        Port = dbMysql.Port,
                                        User = dbMysql.User
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        }

                    }

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(db)) {
                        if (!dbc.CheckDatabase(dbName)) {
                            dbc.CreateDatabase(dbName);
                        }
                    }

                    return Success();

                }
                //return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("GetOrmPlatformList")]
        public string GetOrmPlatformList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    //获取Orm配置文件
                    string xmlString = dpz.Net.Http.GetUTF8($"{site.Config.Orm.XmlUrl}/Setting.xml");

                    using (dpz.Data.Xml xml = new dpz.Data.Xml(xmlString)) {
                        var xmlDB = xml["database"];

                        //建立输出对象
                        var platforms = new List<dpz.Dynamic>();
                        JData.List = platforms;

                        foreach (var xmlPlatform in xmlDB.Nodes) {

                            var plmName = xmlPlatform.Attr["name"];

                            if (xmlPlatform.Name == "platform" && plmName != "Aos") {

                                var platform = new dpz.Dynamic();
                                //var tables = new List<dpz.Dynamic>();
                                platform["Name"] = xmlPlatform.Attr["name"];
                                platform["Title"] = xmlPlatform.Attr["title"];
                                platform["Selected"] = "";
                                //platform["Tables"] = tables;
                                platforms.Add(platform);
                            }
                        }
                    }


                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("GetOrmList")]
        public string GetOrmList() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    string platform = "" + Request.Form["platform"];
                    dpz.Database db;

                    if (aid <= 0) return Fail("未找到授权信息");

                    //合成数据库连接信息
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[AosAuthorize]&[ID=='{aid}']}}");
                        if (row.IsEmpty) return Fail("授权信息无效");

                        if (row["DBIP"] != "") {
                            switch (row["DBType"].ToLower()) {
                                case "sqlserver":
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = row["DBIP"],
                                        Name = "Aos_" + row["DBSign"],
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                case "mysql":
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = row["DBIP"],
                                        Name = "Aos_" + row["DBSign"],
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        } else {
                            var dbAos = site.Config.Database.Aos;
                            switch (dbAos.Type) {
                                case DatabaseTypes.Microsoft_SQL_Server:
                                    var dbSql = dbAos as dpz.Gdbc.Databases.MicrosoftSqlServer;
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = dbSql.Address,
                                        Name = "Aos_" + row["DBSign"],
                                        Password = dbSql.Password,
                                        Port = dbSql.Port,
                                        User = dbSql.User
                                    };
                                    break;
                                case DatabaseTypes.MySQL:
                                    var dbMysql = dbAos as dpz.Gdbc.Databases.MySql;
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = dbMysql.Address,
                                        Name = "Aos_" + row["DBSign"],
                                        Password = dbMysql.Password,
                                        Port = dbMysql.Port,
                                        User = dbMysql.User
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        }

                    }

                    //获取Orm配置文件
                    string xmlString = dpz.Net.Http.GetUTF8($"{site.Config.Orm.XmlUrl}/Setting.xml");

                    using (dpz.Data.Xml xml = new dpz.Data.Xml(xmlString)) {
                        var xmlDB = xml["database"];

                        //建立输出对象
                        var tables = new List<dpz.Dynamic>();
                        JData.List = tables;

                        foreach (var xmlPlatform in xmlDB.Nodes) {

                            var plmName = xmlPlatform.Attr["name"];

                            if (xmlPlatform.Name == "platform" && plmName == platform) {

                                //var platform = new dpz.Dynamic();
                                //var tables = new List<dpz.Dynamic>();
                                //platform["Name"] = xmlPlatform.Attr["name"];
                                //platform["Title"] = xmlPlatform.Attr["title"];
                                //platform["Tables"] = tables;
                                //platforms.Add(platform);

                                //var xmlAos = xmlDB.GetNodeByAttrValue("name", xmlNeeds[i], false);
                                foreach (var xmlTable in xmlPlatform.Nodes) {
                                    if (xmlTable.Name == "table") {

                                        var table = new dpz.Dynamic();
                                        table["Name"] = xmlTable.Attr["name"];
                                        table["Version"] = xmlTable.Attr["version"];
                                        table["PlatformName"] = xmlPlatform.Attr["name"];
                                        table["PlatformTitle"] = xmlPlatform.Attr["title"];
                                        table["SetupVersion"] = "";
                                        tables.Add(table);

                                        string tabName = xmlTable.Attr["name"];
                                        string tabVer = xmlTable.Attr["version"];
                                        using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(db)) {
                                            //var row = dbc.GetOne($"Select * From sysObjects Where Name ='{tabName}' And Type In ('S','U')");
                                            var rowVer = "";
                                            //var opearte = "<i>无需更新</i>";
                                            if (dbc.CheckTable(tabName)) {
                                                var rowObject = dbc.GetGdmlOne($"@{{$[SystemObjects]&[Name=='{tabName}'&&Type=='Table']}}");
                                                if (!rowObject.IsEmpty) {
                                                    rowVer = rowObject["Version"];
                                                    table["SetupVersion"] = rowVer;
                                                }
                                            }

                                            table["NeedUpdate"] = (rowVer == tabVer ? "none" : "");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return Success();
            } catch (Exception ex) {
                return Fail(ex.Message);
            }

        }

        [HttpPost("UpdateOrmTable")]
        public string UpdateOrmTable() {
            try {
                string sid = "" + Request.Form["sid"];
                using (SiteSession session = new SiteSession(sid)) {

                    long aid = ("" + Request.Form["aid"]).ToLong();
                    dpz.Database db;

                    if (aid <= 0) return Fail("未找到授权信息");

                    //合成数据库连接信息
                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(site.Config.Database.Aos)) {
                        var row = dbc.GetGdmlOne($"@{{$[AosAuthorize]&[ID=='{aid}']}}");
                        if (row.IsEmpty) return Fail("授权信息无效");

                        if (row["DBIP"] != "") {
                            switch (row["DBType"].ToLower()) {
                                case "sqlserver":
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = row["DBIP"],
                                        Name = "Aos_" + row["DBSign"],
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                case "mysql":
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = row["DBIP"],
                                        Name = "Aos_" + row["DBSign"],
                                        Password = row["DBPwd"],
                                        Port = row["DBPort"].ToInteger(),
                                        User = row["DBUser"]
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        } else {
                            var dbAos = site.Config.Database.Aos;
                            switch (dbAos.Type) {
                                case DatabaseTypes.Microsoft_SQL_Server:
                                    var dbSql = dbAos as dpz.Gdbc.Databases.MicrosoftSqlServer;
                                    db = new dpz.Gdbc.Databases.MicrosoftSqlServer() {
                                        Address = dbSql.Address,
                                        Name = "Aos_" + row["DBSign"],
                                        Password = dbSql.Password,
                                        Port = dbSql.Port,
                                        User = dbSql.User
                                    };
                                    break;
                                case DatabaseTypes.MySQL:
                                    var dbMysql = dbAos as dpz.Gdbc.Databases.MySql;
                                    db = new dpz.Gdbc.Databases.MySql() {
                                        Address = dbMysql.Address,
                                        Name = "Aos_" + row["DBSign"],
                                        Password = dbMysql.Password,
                                        Port = dbMysql.Port,
                                        User = dbMysql.User
                                    };
                                    break;
                                default:
                                    return Fail($"不支持的数据库类型'{row["DBType"]}'");
                            }
                        }

                    }

                    string plmName = "" + Request.Form["platform"];
                    string tabName = "" + Request.Form["table"];

                    if (tabName != "SystemObjects") {
                        using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(db)) {
                            if (!dbc.CheckTable("SystemObjects")) {
                                return Fail($"请先安装SystemObjects表");
                            }
                        }
                    }

                    string tabVersion = "";
                    string xmlSetting = dpz.Net.Http.GetUTF8($"{site.Config.Orm.XmlUrl}/Setting.xml");
                    using (dpz.Data.Xml xml = new dpz.Data.Xml(xmlSetting)) {
                        var xmlDB = xml["database"];
                        var xmlAos = xmlDB.GetNodeByAttrValue("name", plmName, false);
                        var xmlTable = xmlAos.GetNodeByAttrValue("name", tabName, false);
                        tabVersion = xmlTable.Attr["version"];
                    }

                    string xmlString = dpz.Net.Http.GetUTF8($"{site.Config.Orm.XmlUrl}/{plmName}/{tabName}.xml");

                    if (xmlString == "") {
                        return Fail("配置获取失败,请检查表名称是否存在");
                    }

                    using (dpz.Gdbc.Connection dbc = new dpz.Gdbc.Connection(db)) {

                        string sql = "";

                        using (dpz.Data.Xml xml = new dpz.Data.Xml(xmlString)) {
                            var xmlTable = xml["table"];

                            //判断表是否存在
                            //if (!dbc.GetOne($"Select * From sysObjects Where Name ='{tabName}' And Type In ('S','U')").HasData) {
                            if (!dbc.CheckTable(tabName)) {
                                //添加表
                                List<dpz.Gdbc.TableFieldDefine> fields = new List<dpz.Gdbc.TableFieldDefine>();

                                foreach (var xmlField in xmlTable.Nodes) {
                                    if (xmlField.Name.ToLower() == "field") {
                                        string fieldName = xmlField.Attr["name"];
                                        var xmlData = xmlField["data"];
                                        string fieldDataType = xmlData.Attr["type"].ToLower();
                                        int fieldDataSize = xmlData.Attr["size"].ToInteger();
                                        int fieldDataFloat = xmlData.Attr["float"].ToInteger();

                                        var field = new dpz.Gdbc.TableFieldDefine();
                                        field.Name = fieldName;
                                        field.Type = fieldDataType;
                                        field.Size = fieldDataSize;
                                        field.Float = fieldDataFloat;
                                        fields.Add(field);
                                    }
                                }

                                dbc.CreateTable(tabName, fields);
                            } else {
                                //更新字段
                                foreach (var xmlField in xmlTable.Nodes) {
                                    if (xmlField.Name.ToLower() == "field") {
                                        string fieldName = xmlField.Attr["name"];
                                        var xmlData = xmlField["data"];
                                        string fieldDataType = xmlData.Attr["type"].ToLower();
                                        int fieldDataSize = xmlData.Attr["size"].ToInteger();
                                        int fieldDataFloat = xmlData.Attr["float"].ToInteger();

                                        dpz.Gdbc.TableFieldDefine fieldDefine = new dpz.Gdbc.TableFieldDefine() {
                                            Name = fieldName,
                                            Type = fieldDataType,
                                            Size = fieldDataSize,
                                            Float = fieldDataFloat
                                        };

                                        if (!dbc.CheckTableFiled(tabName, fieldName)) {
                                            dbc.AddTableFiled(tabName, fieldDefine);
                                        } else {
                                            dbc.UpdateTableFiled(tabName, fieldName, fieldDefine);
                                        }
                                    }
                                }
                            }

                            //更新表格结构信息
                            if (dbc.GetGdmlOne($"@{{$[SystemObjects]&[Name=='{tabName}'&&Type=='Table']}}").IsEmpty) {

                                string guid = "";
                                do {
                                    guid = Guid.NewGuid().ToString();
                                } while (!dbc.GetGdmlOne($"@{{$[SystemObjects]&[Guid=='{guid}']}}").IsEmpty);

                                dbc.ExecGdml($"+{{$[SystemObjects].[Name='{tabName}'].[Type='Table'].[Version='{tabVersion}'].[Guid='{guid}']}}");
                            } else {
                                dbc.ExecGdml($"!{{$[SystemObjects].[Version='{tabVersion}']&[Name=='{tabName}'&&Type=='Table']}}");
                            }

                            //JResponse["Version"] = tabVersion;
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