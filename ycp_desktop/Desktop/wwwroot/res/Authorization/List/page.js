/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.list = [];
});

app.ready(function (obj) {
    console.log("Authorization list is loaded!");

    var pg = {};

    //加载列表
    pg.loadList = function () {

        desktop.wait.show("正在加载授权列表");
        //获取用户信息
        ycc.sendJttp("Authorize.GetList", {}, function (e) {
            var status = parseInt(e.Header.Status);
            if (status > 0) {
                obj.list = e.Data.List;
                obj.refresh();
            }
        });
    };

    obj.onAuthChange = function (e) {
        console.log(e);
        var active = parseInt(e.data.Active);
        if (active !== 1) {
            //激活授权
            ycc.sendJttp("Authorize.Active", { AuthID: e.data.ID }, function (e) {
                var status = parseInt(e.Header.Status);
                if (status > 0) {
                    pg.loadList();
                }
            });
        }
    };

    pg.loadList();

});
