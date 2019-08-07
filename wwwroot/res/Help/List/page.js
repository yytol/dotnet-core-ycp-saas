/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />
/// <reference path="../../desktop/index/page.js" />

app.loading(function (obj) {
    obj.data.content = "";
    obj.data.menus = [
        { id: 1, title: "产品信息", url: "/Help/Auth", selected: "no" },
        { id: 10, title: "内核信息", url: "/Help/Core", selected: "no" },
        { id: 11, title: "云桌面信息", url: "/Help/Desktop", selected: "no" }
    ];
});

app.ready(function (obj) {
    //console.log("Authorization manage list is loaded!");

    var pg = {};

    pg.vue = null;

    //产品Vue设定
    pg.authVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "产品信息",
            info: ycc.entityConfig,
            pro: ycc.desktopConfig
        },
        methods: {}
    };

    //内核Vue设定
    pg.coreVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "内核信息",
            info: ycc.info,
            version: ycc.info.getLatestVersion()
        },
        methods: {}
    };

    //云桌面Vue设定
    pg.desktopVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "云桌面信息",
            info: app.info,
            version: app.info.getLatestVersion()
        },
        methods: {}
    };

    //vue绑定数据
    pg.vues = [
        { id: 1, vue: pg.authVue },
        { id: 10, vue: pg.coreVue },
        { id: 11, vue: pg.desktopVue }
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
