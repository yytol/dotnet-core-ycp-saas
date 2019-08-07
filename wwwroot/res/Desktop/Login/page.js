/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycc/ycc_1.1.1902.2.js" />
/// <reference path="../../../js/load.js" />

app.ready(function (obj) {
    console.log("BlueDesktop login is loaded!");

    obj.data.bannerStyle = {
        backgroundImage: "url(" + obj.data.Images.background + ")"
    };

    obj.data.userName = "" + localStorage.getItem("login_user");

    //加载主桌面
    var loadMain = function () {
        //desktop.progress.show(3 / 5, "用户登录成功!", function () {
        desktop.dialog.close();
        //app.unload(ycc.desktopConfig.UrlEntrance, "/res/Desktop/Login/config.json");
        //隐藏底部信息动画
        $("#dpz_Desktop_Info").css({ opacity: 0, bottom: "0px", display: "block" }).animate({ opacity: 0, bottom: "0px" }, 300, function () {
            $("#dpz_Desktop_Info").css({ display: "none" });
            desktop.wait.show("正在加载主界面");
            //desktop.progress.show(4 / 5, "正在加载主界面...", function () {
            app.load(ycc.desktopConfig.UrlEntrance, "/res/Desktop/Main/config.json", $("#dpz_Desktop_Workspace")[0]);
            //});
        });
        //});
    };

    //登录
    //var ;

    obj.methods = {
        //登录
        doLogin: function () {

            desktop.wait.show("正在进行用户登录");
            //desktop.progress.show(2 / 5, "正在进行用户登录...", function () {

            var loginUser = $("#dpz_Desktop_Login_User").val();
            var loginPwd = $("#dpz_Desktop_Login_Pwd").val();

            ycc.socketAutoMessage = false;
            ycc.sendJttp("User.Login", { Name: loginUser, Pwd: loginPwd }, function (e) {
                ycc.socketAutoMessage = true;
                var status = parseInt(e.Header.Status);
                if (status > 0) {
                    //记录本次登录用户
                    localStorage.setItem("login_user", loginUser);
                    //获取用户信息
                    ycc.sendJttp("User.GetInfo", {}, function (ee) {
                        var status = parseInt(ee.Header.Status);
                        if (status > 0) {
                            app.info.user = ee.Data;
                            loadMain();
                        } else {
                            alert("获取用户信息失败!");
                        }
                    });
                    //loadMain();
                } else {
                    $("#dpz_Desktop_Login_Notice").html(e.Message);
                    desktop.wait.hide();
                }
            });
            //});
        },
        doForgetUser: function () {
            alert("用户名通常为您的手机号码，如无法回忆，可联系您的网络管理员或客服QQ：2208899966");
        },
        doForgetPwd: function () {
            alert("请联系您的网络管理员或客服QQ：2208899966");
        }
    };
    //$("#dpz_Desktop_Login_Link").click(doLogin);
    //desktop.load(ycc.desktopConfig.UrlEntrance,"/res/");
    //$("#dpz_Desktop_Login_Box").css({ backgrounfImage: "url(" + app.getImageSrc(obj, "loginBg") + ")" });
});
