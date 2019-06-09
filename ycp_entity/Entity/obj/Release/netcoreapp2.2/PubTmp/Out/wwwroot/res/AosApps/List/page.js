/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.data.catalogs = [];
    obj.data.list = [];
});

app.ready(function (obj) {
    console.log("Authorization manage list is loaded!");

    var pg = {};

    //加载授权列表
    pg.loadAosAuthorizeList = function () {
        ////获取用户信息
        //ycc.sendJttp("Authorize.GetList", {}, function (e) {
        //    var status = parseInt(e.Header.Status);
        //    if (status > 0) {
        //        obj.list = e.Data.List;
        //        obj.refresh();
        //    }
        //});
        desktop.wait.show("正在加载授权列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosAuthorize/GetList", {
            sid: ycc.entityConfig.SessionID
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                obj.data.catalogs = jttp.Data.List;

                //设置选择状态
                for (var i = 0; i < obj.data.catalogs.length; i++) {
                    obj.data.catalogs[i].Selected = "";
                }
                //obj.refresh();
            }
            desktop.wait.hide();
        });
    };

    //加载列表
    pg.loadList = function (aid) {
        ////获取用户信息
        //ycc.sendJttp("Authorize.GetList", {}, function (e) {
        //    var status = parseInt(e.Header.Status);
        //    if (status > 0) {
        //        obj.list = e.Data.List;
        //        obj.refresh();
        //    }
        //});

        //设置选择状态
        for (var i = 0; i < obj.data.catalogs.length; i++) {
            if (obj.data.catalogs[i].ID === aid) {
                obj.data.catalogs[i].Selected = "yes";
            } else {
                obj.data.catalogs[i].Selected = "";
            }
        }

        desktop.wait.show("正在加载应用列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosApps/GetList", {
            sid: ycc.entityConfig.SessionID,
            aid: aid
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                obj.data.list = jttp.Data.List;
                //obj.refresh();
            }
            desktop.wait.hide();
        });
    };

    obj.methods = {
        onChangeCatalog: function (e, ob) {
            pg.loadList(ob.ID);
        },
        onMoveUp: function (e, ob) {
            desktop.wait.show("正在处理排序");
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosApps/MoveUp", {
                sid: ycc.entityConfig.SessionID,
                ID: ob.ID,
                AuthID: ob.AuthID
            }, function (e) {
                var jttp = dpz.data.json.parse(e);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    //obj.data.list = jttp.Data.List;
                    //obj.refresh();
                    if (!dpz.isNull(jttp.Message)) alert(jttp.Message);
                    pg.loadList(ob.AuthID);
                } else {
                    if (!dpz.isNull(jttp.Message)) alert(jttp.Message);
                }
                desktop.wait.hide();
            });
        },
        onMoveDown: function (e, ob) {
            desktop.wait.show("正在处理排序");
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosApps/MoveDown", {
                sid: ycc.entityConfig.SessionID,
                ID: ob.ID,
                AuthID: ob.AuthID
            }, function (e) {
                var jttp = dpz.data.json.parse(e);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    //obj.data.list = jttp.Data.List;
                    //obj.refresh();
                    if (!dpz.isNull(jttp.Message)) alert(jttp.Message);
                    pg.loadList(ob.AuthID);
                } else {
                    if (!dpz.isNull(jttp.Message)) alert(jttp.Message);
                }
                desktop.wait.hide();
            });
        }
    };

    obj.methods.onAdd = function (e) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/AosApps/Add/config.json", {
            hasHeader: true,
            width: 480,
            height: 340,
            title: "添加应用仓库",
            args: {}
        });
    };

    obj.methods.onEdit = function (e, ob) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        //console.log(e);
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/AosApps/Edit/config.json", {
            hasHeader: true,
            width: 480,
            height: 340,
            title: "修改应用仓库 - [" + ob.Name + "]",
            args: ob
        });
    };

    obj.methods.onRefresh = function (e) {
        pg.loadList();
    };

    obj.methods.onDelete = function (e, ob) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        //console.log(e);
        if (confirm("确定要删除项目 \"" + ob.Name + "\" 吗?")) {
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosApps/Delete", {
                sid: ycc.entityConfig.SessionID,
                ID: ob.ID
            }, function (re) {
                var jttp = dpz.data.json.parse(re);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    desktop.delayed.show("操作成功", 2000);
                    pg.loadList();
                } else {
                    if (jttp.Message !== "") alert(jttp.Message);
                }
            });
        }
    };

    obj.methods.onSetup = function (e, ob) {
        //console.log(e);
        //console.log(ob);
        desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/AosApps/Setup/config.json", ob);
    };

    //pg.loadList();
    setTimeout(pg.loadAosAuthorizeList, 10);

});
