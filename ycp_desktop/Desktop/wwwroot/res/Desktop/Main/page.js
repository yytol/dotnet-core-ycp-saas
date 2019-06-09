/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycp/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />
/// <reference path="../index/page.js" />

app.onLoad(function (obj) {
    obj.data.User = {
        Nick: app.info.user.Nick,
        Name: app.info.user.Name
    };
});

app.ready(function (obj) {
    console.log("BlueDesktop main is loaded!");

    //添加循环队列
    console.log("Add session keep queue");
    ycc.queue.add(function () {

        //发送交互信息保持申请
        ycc.sendJttp("Session.Keep", null, function (e) {
            var status = parseInt(e.Header.Status);
            if (status <= 0) {
                alert("交互信息超时或异常,请刷新页面重试");
            }
        });

    }, 10);

    var pg = {};

    $("#dpz_Desktop_Main_Header").animate({ top: "0px", opacity: 1 }, 300, function () {
        $("#dpz_Desktop_Main_Menu").animate({ left: "0px", opacity: 1 }, 300, function () {
            $("#dpz_Desktop_Main_Body").animate({ right: "0px", opacity: 1 }, 300, function () {

                desktop.wait.show("加载用户可用授权");
                //desktop.progress.show(5 / 6, "加载已激活操作对象...", function () {
                //获取用户授权信息
                ycc.sendJttp("Authorize.GetList", { DesktopID: ycc.desktopConfig.ID }, function (e) {
                    var status = parseInt(e.Header.Status);
                    if (status > 0) {
                        var list = e.Data.List;

                        if (list.length <= 0) {
                            alert("尚未发现可用授权信息");
                            desktop.wait.hide();
                        } else if (list.length === 1) {
                            //只有一个可用授权时，直接进入主界面
                            ycc.entityConfig = list[0];
                            dpz.loader.loadJs("Aos_Js_Entitiy", ycc.entityConfig.UrlEntrance + ycc.entityConfig.ScriptEntrance);
                        } else {
                            //当有多个可用授权时，弹出对话框让客户选择
                        }

                        //console.log(ycc.entityConfig);
                        //desktop.wait.hide();
                        //desktop.wait.show("正在加载");

                        //desktop.progress.show(5 / 5, "加载完成!", function () {
                        //    desktop.progress.hide();
                        //});
                    }
                });
                //});
            });
        });
    });

    //绑定概览按钮
    $("#dpz_Desktop_Main_Overview_Link").click(function () {
        desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Overview/Home/config.json");
    });

    //绑定消息按钮
    $("#dpz_Desktop_Main_Message_Link").click(function () {
        desktop.body.show(ycc.desktopConfig.UrlEntrance, "/res/Message/List/config.json");
    });

    //绑定设置按钮
    $("#dpz_Desktop_Main_Setting_Link").click(function () {
        desktop.body.show(ycc.desktopConfig.UrlEntrance, "/res/Setting/List/config.json");
    });

    //绑定关于按钮
    $("#dpz_Desktop_Main_Help_Link").click(function () {
        desktop.body.show(ycc.desktopConfig.UrlEntrance, "/res/Help/List/config.json");
    });

    //绑定工具栏退出按钮
    $("#dpz_Desktop_Main_Exit_Link").click(function () {
        ycc.sendJttp("User.Logout", {}, function (e) {
            var status = parseInt(e.Header.Status);
            if (status > 0) { location.reload(true); }
        });
    });

    var menuHeaderStatus = -1;

    desktop.closeMenu = function () {
        menuHeaderStatus = 0;
        $("#dpz_Desktop_Main_Menu").animate({ width: "50px" }, 200, function () {
            menuHeaderStatus = -1;
        });
    };

    $("#dpz_Desktop_Main_Menu_Header").click(function () {
        if (menuHeaderStatus === 0) return;
        if (menuHeaderStatus < 0) {
            menuHeaderStatus = 0;
            $("#dpz_Desktop_Main_Menu").animate({ width: "220px" }, 200, function () {
                menuHeaderStatus = 1;
            });
        } else {
            desktop.closeMenu();
        }
    });
    //desktop.load(ycc.desktopConfig.UrlEntrance,"/res/");
    //$("#dpz_Desktop_Login_Box").css({ backgrounfImage: "url(" + app.getImageSrc(obj, "loginBg") + ")" });
});
