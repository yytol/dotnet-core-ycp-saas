/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.data.list = [];
});

app.ready(function (obj) {
    console.log("Authorization manage list is loaded!");

    var pg = {};

    //加载列表
    pg.loadList = function () {
        ////获取用户信息
        //ycc.sendJttp("Authorize.GetList", {}, function (e) {
        //    var status = parseInt(e.Header.Status);
        //    if (status > 0) {
        //        obj.list = e.Data.List;
        //        obj.refresh();
        //    }
        //});
        desktop.wait.show("正在加载授权列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Desktop/GetList", {
            sid: ycc.entityConfig.SessionID
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

    pg.getDetect = function (id, fn) {
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/CheckOrmDatebase", {
            sid: ycc.entityConfig.SessionID,
            aid: id
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            for (var i = 0; i < obj.data.list.length; i++) {
                if (obj.data.list[i].ID === id) {
                    if (status > 0) {
                        obj.data.list[i].DBStatus = "yes";
                    } else {
                        obj.data.list[i].DBStatus = "no";
                    }
                    break;
                }
            }
            if (dpz.isFunction(fn)) fn();
        });
    };

    obj.methods = {};

    obj.methods.onAdd = function (e) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/Desktop/Add/config.json", {
            hasHeader: true,
            width: 480,
            height: 340,
            title: "添加授权",
            args: {}
        });
    };

    obj.methods.onEdit = function (e, ob) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        //console.log(e);
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/Desktop/Edit/config.json", {
            hasHeader: true,
            width: 480,
            height: 340,
            title: "修改授权 - [" + ob.Text + "]",
            args: ob
        });
    };

    obj.methods.onRefresh = function (e) {
        pg.loadList();
    };

    obj.methods.onDetect = function (e) {
        desktop.wait.show("正在进行安装数据库检测");
        for (var i = 0; i < obj.data.list.length; i++) {
            if (obj.data.list[i].DBStatus === "") {
                pg.getDetect(obj.data.list[i].ID, function () {
                    obj.methods.onDetect(e);
                });
                return;
            }
        }
        desktop.wait.hide();
    };

    obj.methods.onDelete = function (e, ob) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        //console.log(e);
        if (confirm("确定要删除项目 \"" + ob.Name + "\" 吗?")) {
            $.post(ycc.entityConfig.UrlEntrance + "/Api/Desktop/Delete", {
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
        desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Setup/config.json", ob);
    };

    obj.methods.onOneDetect = function (e, ob) {
        //pg.loadList();
        pg.getDetect(ob.ID);
    };

    obj.methods.onOneSetup = function (e, ob) {
        //pg.loadList();
        desktop.wait.show("正在进行数据库安装");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/CreateOrmDatebase", {
            sid: ycc.entityConfig.SessionID,
            aid: ob.ID
        }, function (e) {
            var jttp = dpz.data.json.parse(e);
            //console.log(jttp);
            var status = parseInt(jttp.Header.Status);
            for (var i = 0; i < obj.data.list.length; i++) {
                if (obj.data.list[i].ID === ob.ID) {
                    if (status > 0) {
                        obj.data.list[i].DBStatus = "yes";
                    } else {
                        obj.data.list[i].DBStatus = "no";
                    }
                    break;
                }
            }
            desktop.wait.hide();
        });
    };

    //pg.loadList();
    setTimeout(pg.loadList, 10);

});
