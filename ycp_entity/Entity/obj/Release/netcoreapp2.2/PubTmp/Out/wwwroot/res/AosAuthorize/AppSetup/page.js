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

    pg.getList = function () {
        //obj.info = "正在获取表格列表...";
        //obj.refresh();
        desktop.wait.show("正在获取分类信息列表");
        $.post(ycc.entityConfig.UrlEntrance + "/Api/AosAuthorize/GetAppList", {
            sid: ycc.entityConfig.SessionID,
            aid: obj.data.Args.ID,
            url: obj.data.Args.UrlEntrance + obj.data.Args.AppConfigEntrance
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

    obj.methods = {
        goList: function (e) {
            desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/AosAuthorize/List/config.json");
        },
        onUpdate: function (e, ob) {
            //obj.info = "正在更新数据表...";
            //obj.refresh();
            desktop.wait.show("正在安装或更新数据表");
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosAuthorize/UpdateApp", {
                sid: ycc.entityConfig.SessionID,
                aid: obj.data.Args.ID,
                sign: ob.Name,
                url: obj.data.Args.UrlEntrance + obj.data.Args.AppConfigEntrance
            }, function (e) {
                var jttp = dpz.data.json.parse(e);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    pg.getList();
                } else {
                    alert(jttp.Message);
                    //obj.refresh();
                    desktop.wait.hide();
                }

            });
        }
    };

    setTimeout(pg.getList, 10);

});
