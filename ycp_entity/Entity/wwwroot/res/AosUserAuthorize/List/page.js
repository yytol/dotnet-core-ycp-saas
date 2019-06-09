/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.data.users = [];
    obj.data.catalogs = [];
    obj.data.list = [];
});

app.ready(function (obj) {
    console.log("Authorization manage list is loaded!");

    var pg = {};

    pg.userId = 0;
    pg.authId = 0;

    //加载用户列表
    pg.loadAosUsersList = function () {
        desktop.wait.show("正在加载用户列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUsers/GetList", {
            sid: ycc.entityConfig.SessionID
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                obj.data.users = jttp.Data.List;

                //设置选择状态
                for (var i = 0; i < obj.data.users.length; i++) {
                    obj.data.users[i].Selected = "";
                }
                //obj.refresh();
            }
            desktop.wait.hide();
            //pg.loadAosAuthorizeList();
        });
    };

    //加载列表
    pg.loadList = function (uid, aid) {

        //设置用户选择状态
        for (var i = 0; i < obj.data.users.length; i++) {
            if (obj.data.users[i].ID === uid) {
                obj.data.users[i].Selected = "yes";
            } else {
                obj.data.users[i].Selected = "";
            }
        }

        desktop.wait.show("正在加载授权列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUserAuthorize/GetList", {
            sid: ycc.entityConfig.SessionID,
            uid: uid,
            aid: aid
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            if (status > 0) {
                obj.data.list = jttp.Data.List;
                //obj.refresh();
            } else {
                if (jttp.Message !== "") alert(jttp.Message);
            }
            desktop.wait.hide();
            //setTimeout(app.vues.Page.$forceUpdate, 100);
        });
    };

    obj.methods = {
        onChangeCatalog: function (e, ob) {
            pg.authId = ob.ID;
            pg.loadList(pg.userId, pg.authId);
        },
        onChangeUser: function (e, ob) {
            pg.userId = ob.ID;
            pg.loadList(pg.userId, pg.authId);
        },
        onCheck: function (e, ob) {
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUserAuthorize/Save", {
                sid: ycc.entityConfig.SessionID,
                UserID: pg.userId,
                AuthID: ob.ID
            }, function (re) {
                var jttp = dpz.data.json.parse(re);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    //desktop.delayed.show("操作成功", 2000);
                    //pg.loadList();
                    for (var i = 0; i < obj.data.list.length; i++) {
                        if (obj.data.list[i].ID === ob.ID) {
                            obj.data.list[i].Check = jttp.Data.Check;
                            break;
                        }
                    }
                } else {
                    if (jttp.Message !== "") alert(jttp.Message);
                }
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
    setTimeout(pg.loadAosUsersList, 10);

});
