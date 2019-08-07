/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />
/// <reference path="../../desktop/index/page.js" />

app.loading(function (obj) {
    obj.data.content = "";
    obj.data.menus = [
        { id: 1, title: "密码修改", url: "/Setting/Pwd", selected: "no" }
    ];
});

app.ready(function (obj) {
    //console.log("Authorization manage list is loaded!");

    var pg = {};

    pg.vue = null;

    pg.pwdVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Name: "密码修改",
            form: {
                OldPwd: "",
                NewPwd: "",
                RePwd: ""
            }
        },
        methods: {
            onSubmit: function () {
                //获取用户信息
                ycc.sendJttp("User.Repwd", pg.pwdVue.data.form, function (ee) {
                    var status = parseInt(ee.Header.Status);
                    if (status > 0) {
                        pg.pwdVue.data.form.OldPwd = "";
                        pg.pwdVue.data.form.NewPwd = "";
                        pg.pwdVue.data.form.RePwd = "";
                        desktop.delayed.show("密码修改成功!", 2000);
                    } else {
                        alert(ee.Message);
                    }
                });
                //alert("提交");
            }
        }
    };

    //vue绑定数据
    pg.vues = [
        { id: 1, vue: pg.pwdVue }
    ];

    //加载用户列表
    pg.loadPage = function (id) {

        desktop.wait.show("正在加载子页面");

        if (!dpz.isNull(pg.vue)) {
            pg.vue.$destroy();
            pg.vue = null;
        }

        //设置用户选择状态
        var ob = null;
        for (var i = 0; i < obj.data.menus.length; i++) {
            if (obj.data.menus[i].id === id) {
                ob = obj.data.menus[i];
                obj.data.menus[i].selected = "yes";
            } else {
                obj.data.menus[i].selected = "no";
            }
        }

        var obVue = pg.vues[0];
        for (i = 0; i < pg.vues.length; i++) {
            if (pg.vues[i].id === id) {
                obVue = pg.vues[i];
                break;
            }
        }

        //console.log(ob);

        $.get(ycc.desktopConfig.UrlEntrance + ob.url, function (data) {
            $("#dpz_Desktop_Setting_List_Content").html(data);
            pg.vue = new Vue(obVue.vue);
            desktop.wait.hide();
        });

    };

    obj.methods = {
        onMenu: function (e, ob) {
            pg.loadPage(ob.id);
        }
    };

    //pg.loadList();
    setTimeout(function () { pg.loadPage(1); }, 10);

});
