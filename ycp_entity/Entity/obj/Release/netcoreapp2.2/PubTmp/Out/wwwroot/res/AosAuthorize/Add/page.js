/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

app.loading(function (obj) {
    obj.data.selector = {};
});

app.ready(function (obj) {
    console.log("Authorization manage list is loaded!");

    var pg = {};

    obj.methods = {
        onSave: function (e) {
            var form = dpz.element.getFormById("dpz_Desktop_Dialog_Body");
            var args = form.getValues();
            args.ID = "0";
            args.sid = ycc.entityConfig.SessionID;
            //console.log(args);
            $.post(ycc.entityConfig.UrlEntrance + "/Api/AosAuthorize/Save", args, function (re) {
                var jttp = dpz.data.json.parse(re);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    desktop.delayed.show("操作成功", 2000);
                    desktop.dialog.close();
                    //刷新列表应用数据
                    app.configs.ManageAosAuthorizeList.methods.onRefresh();
                } else {
                    if (jttp.Message !== "") alert(jttp.Message);
                }
            });
        },
        onCancel: function (e) {
            desktop.dialog.close();
        }
    };

    //setTimeout(pg.runSelect, 10);
    //console.log(obj);

});
