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
            var form = dpz.element.getFormById("dpz_Manage_Authorization_Add_Form");
            var args = form.getValues();
            args.ID = obj.data.Args.ID;
            args.sid = ycc.entityConfig.SessionID;
            //console.log(args);
            $.post(ycc.entityConfig.UrlEntrance + "/Api/Auth/Save", args, function (re) {
                var jttp = dpz.data.json.parse(re);
                //console.log(jttp);
                var status = parseInt(jttp.Header.Status);
                if (status > 0) {
                    desktop.delayed.show("操作成功", 2000);
                    desktop.dialog.close();
                    //刷新列表应用数据
                    app.configs.AuthList.methods.onRefresh();
                } else {
                    if (jttp.Message !== "") alert(jttp.Message);
                }
            });
        },
        onCancel: function (e) {
            desktop.dialog.close();
        }
    };

    //处理类型
    pg.runSelect = function () {
        var dbType = obj.data.Args.DBType;
        if (!dpz.isNull(dbType)) {
            if (dbType !== "") {
                var form = document.getElementById("dpz_Manage_Authorization_Add_Form");
                var selectos = form.getElementsByTagName("select");
                for (var i = 0; i < selectos.length; i++) {
                    var select = selectos[i];
                    if (select.name === "DBType") {
                        for (var j = 0; j < select.length; j++) {
                            if (select[j].value === dbType)
                                select[j].selected = true;
                        }
                    }
                }
            }
        }
    };

    setTimeout(pg.runSelect, 10);
    //console.log(obj);

});
