﻿/// <reference path="../../../lib/jquery/dist/jquery.js" />
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
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUsers/GetList", {
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

    obj.methods = {
        onAppSetup: function (e, ob) {
            desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/AosUsers/AppSetup/config.json", ob);
        },
        onRepwd: function (e, ob) {
            //pg.loadList();
            if (confirm("密码初始化无法恢复，确定将用户 \"" + ob.Name + "\" 密码初始化吗?")) {
                desktop.wait.show("正在进行数据库安装");
                $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUsers/Repwd", {
                    sid: ycc.entityConfig.SessionID,
                    ID: ob.ID
                }, function (e) {
                    var jttp = dpz.data.json.parse(e);
                    //console.log(jttp);
                    var status = parseInt(jttp.Header.Status);
                    if (status > 0) {
                        desktop.delayed.show("操作成功", 2000);
                        pg.loadList();
                    } else {
                        if (jttp.Message !== "") alert(jttp.Message);
                    }
                    desktop.wait.hide();
                });
            }
        }
    };

    obj.methods.onAdd = function (e) {
        //desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Authorization/Add/config.json");
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/AosUsers/Add/config.json", {
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
        desktop.dialog.show(ycc.entityConfig.UrlEntrance, "/res/AosUsers/Edit/config.json", {
            hasHeader: true,
            width: 480,
            height: 340,
            title: "修改授权 - [" + ob.Name + "]",
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
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosUsers/Delete", {
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
        desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/AosUsers/Setup/config.json", ob);
    };

    obj.methods.onOneDetect = function (e, ob) {
        //pg.loadList();
        pg.getDetect(ob.ID);
    };

    //pg.loadList();
    setTimeout(pg.loadList, 10);

});
