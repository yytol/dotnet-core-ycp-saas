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
    pg.loadDesktopList = function () {
        desktop.wait.show("正在加载用户列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/Desktop/GetList", {
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
    pg.loadList = function (uid) {

        //设置用户选择状态
        for (var i = 0; i < obj.data.users.length; i++) {
            if (obj.data.users[i].ID === uid) {
                obj.data.users[i].Selected = "yes";
            } else {
                obj.data.users[i].Selected = "";
            }
        }

        desktop.wait.show("正在加载授权列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosDesktopAuthorizes/GetList", {
            sid: ycc.entityConfig.SessionID,
            DesktopID: uid,
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
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosDesktopAuthorizes/Save", {
                sid: ycc.entityConfig.SessionID,
                DesktopID: pg.userId,
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

    //pg.loadList();
    setTimeout(pg.loadDesktopList, 10);

});
