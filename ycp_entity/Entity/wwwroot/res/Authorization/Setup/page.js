/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.data.info = "";
    obj.data.setupButton = "none";
    obj.data.catalogs = [];
    obj.data.list = [];
});

app.ready(function (obj) {

    var pg = {};

    //pg.platform = "";

    pg.getPlatformList = function () {
        //obj.info = "正在获取表格列表...";
        //obj.refresh();
        desktop.wait.show("正在获取分类信息列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/GetOrmPlatformList", {
            sid: ycc.entityConfig.SessionID,
            aid: obj.data.Args.ID
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                //obj.data.info = "获取成功!";
                obj.data.catalogs = jttp.Data.List;
                //obj.refresh();
            } else {
                alert(jttp.Message);
                //obj.refresh();
            }
            desktop.wait.hide();
        });
    };

    pg.getTableList = function (plm) {

        desktop.wait.show("正在获取表格列表");
        //obj.info = "正在获取表格列表...";
        //obj.refresh();

        //设置选择状态
        for (var i = 0; i < obj.data.catalogs.length; i++) {
            if (obj.data.catalogs[i].Name === plm) {
                obj.data.catalogs[i].Selected = "yes";
            } else {
                obj.data.catalogs[i].Selected = "";
            }
        }

        $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/GetOrmList", {
            sid: ycc.entityConfig.SessionID,
            aid: obj.data.Args.ID,
            platform: plm
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                //obj.data.info = "获取成功!";
                obj.data.list = jttp.Data.List;
                //obj.refresh();
            } else {
                alert(jttp.Message);
                //obj.refresh();
            }
            desktop.wait.hide();
        });
    };

    pg.checkOrmDatebase = function () {
        obj.info = "正在检测数据库安装情况...";
        obj.refresh();
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/CheckOrmDatebase", {
            sid: ycc.entityConfig.SessionID,
            aid: obj.Args.ID
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                obj.info = "数据库已安装!";
                obj.refresh();
                pg.getTableList();
            } else {
                obj.info = "数据库未安装!";
                obj.setupButton = "";
                obj.refresh();
            }
        });
    };

    obj.methods = {
        goList: function (e) {
            desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/AosAuthorize/List/config.json");
        },
        onChangeCatalog: function (e, ob) {
            pg.getTableList(ob.Name);
        },
        onCreateDB: function (e) {
            desktop.wait.show("正在安装或更新数据表");
            $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/CreateOrmDatebase", {
                sid: ycc.entityConfig.SessionID,
                aid: obj.Args.ID
            }, function (e) {
                var jttp = dpz.data.json.parse(e);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    obj.info = "数据库已安装!";
                    obj.setupButton = "none";
                    obj.refresh();
                } else {
                    obj.info = "数据库未安装!";
                    obj.setupButton = "";
                    obj.refresh();
                }
                desktop.wait.hide();
            });
        },

        onUpdate: function (e, ob) {
            //obj.info = "正在更新数据表...";
            //obj.refresh();
            desktop.wait.show("正在安装或更新数据表");
            $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/UpdateOrmTable", {
                sid: ycc.entityConfig.SessionID,
                aid: obj.data.Args.ID,
                table: ob.Name,
                platform: ob.PlatformName
            }, function (e) {
                var jttp = dpz.data.json.parse(e);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    pg.getTableList(ob.PlatformName);
                } else {
                    alert(jttp.Message);
                    //obj.refresh();
                    desktop.wait.hide();
                }
                
            });
        }
    };

    setTimeout(pg.getPlatformList, 10);

});
