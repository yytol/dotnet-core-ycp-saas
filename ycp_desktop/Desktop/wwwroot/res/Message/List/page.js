/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />
/// <reference path="../../desktop/index/page.js" />

app.loading(function (obj) {
    obj.data.content = "";
    obj.data.menus = [
        { id: 1, title: "全部消息", url: "/Message/Table", ws: { type: "Message.GetAll", data: { page: 1 } }, selected: "no" },
        { id: 2, title: "未读消息", url: "/Message/Table", ws: { type: "Message.GetNew", data: { page: 1 } }, selected: "no" },
        { id: 3, title: "已读消息", url: "/Message/Table", ws: { type: "Message.GetOld", data: { page: 1 } }, selected: "no" }
    ];
});

app.ready(function (obj) {
    //console.log("Authorization manage list is loaded!");

    var pg = {};

    pg.vue = null;

    //产品Vue设定
    pg.allVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "全部消息",
            List: [],
            PageSize: 10,
            Page: 1,
            RowCount: 1,
            PageCount: 1
        },
        methods: {}
    };

    //内核Vue设定
    pg.newVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "未读消息",
            List: [],
            PageSize: 10,
            Page: 1,
            RowCount: 1,
            PageCount: 1
        },
        methods: {}
    };

    //云桌面Vue设定
    pg.oldpVue = {
        el: "#dpz_Desktop_Setting_List_Content",
        data: {
            Title: "已读消息",
            List: [],
            PageSize: 10,
            Page: 1,
            RowCount: 1,
            PageCount: 1
        },
        methods: {}
    };

    //vue绑定数据
    pg.vues = [
        { id: 1, vue: pg.allVue },
        { id: 2, vue: pg.newVue },
        { id: 3, vue: pg.oldpVue }
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
