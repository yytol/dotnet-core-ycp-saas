/// <reference path="../../../lib/jquery/dist/jquery.js" />
/// <reference path="../../../lib/vue/vue.js" />
/// <reference path="../../../lib/dpz/dpz_1.1.1901.2.js" />
/// <reference path="../../../lib/ycc/ycc.js" />
/// <reference path="../../../js/load.js" />

var desktop = {};

/**
 * 桌面进度条
 * */
desktop.progress = {
    show: function (val, info, fn, wait) {
        var db = parseFloat(val) * 100;
        if (typeof wait === "undefined") wait = 100;
        $("#dpz_Desktop_Progress_Info").html(info);
        $("#dpz_Desktop_Progress_Block").css({ width: db + "%" });
        $("#dpz_Desktop_Progress").css({ display: "block", opacity: 1 });
        if (dpz.isFunction(fn)) setTimeout(fn, wait);
    },
    hide: function () {
        $("#dpz_Desktop_Progress").animate({ opacity: 0 }, 300, function () {
            $("#dpz_Desktop_Progress").css({ display: "none" });
        });
    }
};

/**
 * 桌面延迟提醒框
 * */
desktop.delayed = {
    index: 0,
    show: function (val, time) {
        desktop.delayed.index++;
        var idx = desktop.delayed.index;
        if (typeof time === "undefined") time = 0;
        $("#dpz_Desktop_Delayed_Box").html(val);
        $("#dpz_Desktop_Delayed").css({ opacity: 0, display: "block" }).animate({ opacity: 0.8 }, 300, function () {
            if (time > 0) {
                setTimeout(function () {
                    if (idx === desktop.delayed.index) {
                        $("#dpz_Desktop_Delayed").animate({ opacity: 0 }, 300, function () {
                            $("#dpz_Desktop_Delayed").css({ display: "none" });
                        });
                    }
                }, time);
            }
        });
    }
};

/**
 * 桌面等待提醒框
 * */
desktop.wait = {
    show: function (val) {
        if (typeof val === "undefined") val = "正在加载";
        $("#dpz_Desktop_Wait_Info").html(val);
        $("#dpz_Desktop_Wait").css({ display: "block" });
    },
    hide: function () {
        $("#dpz_Desktop_Wait").css({ display: "none" });
    }
};

/**
 * 桌面进度条
 * */
desktop.tip = {
    bind: function (ele, html) {
    }
};

/**
 * 桌面提示框
 * */
desktop.dialog = {
    vm: null,
    member: { entrance: "", url: "" },
    careteOptions: function () {
        return {
            title: "提示",
            width: 400,
            height: 400,
            hasHeader: true,
            hasBackground: true,
            toTop: false,
            style: {}
        };
    },
    show: function (entrance, url, opt, fn) {
        var that = this;

        var process = function () {

            var cfg = desktop.dialog.careteOptions();
            if (!dpz.isNull(opt)) {
                if (typeof opt.title !== "undefined") cfg.title = opt.title;
                if (typeof opt.width !== "undefined") cfg.width = opt.width;
                if (typeof opt.height !== "undefined") cfg.height = opt.height;
                if (typeof opt.hasHeader !== "undefined") cfg.hasHeader = opt.hasHeader;
                if (typeof opt.hasBackground !== "undefined") cfg.hasBackground = opt.hasBackground;
                for (var k in opt.style) {
                    cfg.style[k] = opt.style[k];
                }
            }


            //设置样式
            $("#dpz_Desktop_Dialog_Box").css(cfg.style);

            if (cfg.hasHeader) {
                $("#dpz_Desktop_Dialog_Header").css({ display: "block" });
                $("#dpz_Desktop_Dialog_Body").css({ height: "calc(100% - 40px)" });
                $("#dpz_Desktop_Dialog_Title").html(cfg.title);
            } else {
                $("#dpz_Desktop_Dialog_Header").css({ display: "none" });
            }

            if (cfg.hasBackground) {
                $("#dpz_Desktop_Dialog_Area").css({ background: "rgba(0,0,0,0.5)" });
            } else {
                $("#dpz_Desktop_Dialog_Area").css({ background: "" });
            }

            var animateMarginTop = "";
            if (cfg.toTop) {
                $("#dpz_Desktop_Dialog_Box").css({ top: "0px" });
                animateMarginTop: "0px";
            } else {
                $("#dpz_Desktop_Dialog_Box").css({ top: "50%" });
                animateMarginTop = "-" + parseInt(opt.height / 2 + 50) + "px";
            }

            $("#dpz_Desktop_Dialog_Box").css({ opacity: 0, width: cfg.width + "px", height: cfg.height + "px", marginLeft: "-" + parseInt(cfg.width / 2) + "px", marginTop: "-" + cfg.height + "px" });
            $("#dpz_Desktop_Dialog_Body").html("");
            $("#dpz_Desktop_Dialog").css({ display: "block" });
            $("#dpz_Desktop_Dialog_Box").animate({ opacity: 1, marginTop: animateMarginTop }, 300, function () {
                that.member.entrance = entrance;
                that.member.url = url;
                app.load(entrance, url, "#dpz_Desktop_Dialog_Body", opt.args, function () {
                    if (dpz.isFunction(fn)) fn();
                });
            });
        };

        if (that.member.url !== "") {
            app.unload(that.member.entrance, that.member.url, function () {
                that.member.entrance = "";
                that.member.url = "";
                process();
            });
        } else {
            process();
        }

    },
    close: function (fn) {
        var that = this;
        $("#dpz_Desktop_Dialog_Box").animate({ opacity: 0 }, 300, function () {
            $("#dpz_Desktop_Dialog").css({ display: "none" });
            if (that.member.url !== "") {
                app.unload(that.member.entrance, that.member.url, function () {
                    that.member.entrance = "";
                    that.member.url = "";
                    if (dpz.isFunction(fn)) fn();
                });
            }
        });
    }
};

/**
 * 桌面主体区域
 * */
desktop.body = {
    vm: null,
    member: { entrance: "", url: "" },
    switchPage: {
        element: "",
        member: { entrance: "", url: "" },
        show: function (entrance, url, args) {

            desktop.wait.show("正在加载应用");

            var that = desktop.body.switchPage;
            var process = function () {
                that.member.entrance = entrance;
                that.member.url = url;
                app.load(entrance, url, $(that.element)[0], args, desktop.wait.hide, desktop.wait.hide);
            };

            //$("#dpz_Entity_TabPage_Page_Content").html("");
            $(that.element).html("");
            if (that.member.url !== "") {
                app.unload(that.member.entrance, that.member.url, function () {
                    that.member.entrance = "";
                    that.member.url = "";
                    process();
                });
            } else {
                process();
            }
        }
    },
    show: function (entrance, url, args) {

        desktop.wait.show("正在加载应用");

        var that = this;
        var process = function () {
            that.member.entrance = entrance;
            that.member.url = url;
            app.load(entrance, url, $("#dpz_Desktop_Main_Body")[0], args, desktop.wait.hide, desktop.wait.hide);
        };

        var processUnload = function () {
            app.unload(that.member.entrance, that.member.url, function () {
                that.member.entrance = "";
                that.member.url = "";
                process();
            });
        };

        $("#dpz_Desktop_Main_Body").html("");
        if (that.member.url !== "") {
            if (that.switchPage.member.url !== "") {
                app.unload(that.switchPage.member.entrance, that.switchPage.member.url, function () {
                    that.switchPage.member.entrance = "";
                    that.switchPage.member.url = "";
                    processUnload();
                });
            } else {
                processUnload();
            }
        } else {
            process();
        }
    }
};

app.loading(function (obj) {

    console.log("BlueDesktop index is loading...");
    //console.log(obj);

    obj.data.Desktop = ycc.desktopConfig;

    var dt = new Date();
    obj.data.Year = dt.getFullYear();

    obj.data.styleWorkspace = "background-image:url(" + obj.data.Images.loginBg + ");";
    obj.data.styleWait = "background-image:url(" + obj.data.Images.loading + ");";
});

app.ready(function () {

    console.log("BlueDesktop index is loaded!");

    desktop.wait.show("正在加载核心组件");

    //加载主桌面
    var loadMain = function () {

        //desktop.progress.show(3 / 5, "用户已经登录!", function () {
        //desktop.dialog.close();
        //隐藏底部信息动画
        $("#dpz_Desktop_Info").css({ opacity: 0, bottom: "0px", display: "block" }).animate({ opacity: 0, bottom: "0px" }, 300, function () {
            $("#dpz_Desktop_Info").css({ display: "none" });

            desktop.wait.show("正在加载主界面");
            //desktop.progress.show(4 / 5, "正在加载主界面...", function () {
            app.load(ycc.desktopConfig.UrlEntrance, "/res/Desktop/Main/config.json", "#dpz_Desktop_Workspace", {});
            //});
        });
        //});
    };

    var loadInfoBar = function () {
        //显示底部信息动画
        $("#dpz_Desktop_Info").css({ opacity: 0, bottom: "0px", display: "block" }).animate({ opacity: 1, bottom: "50px" }, 300, function () {

            desktop.wait.show("正在获取用户登录信息");
            //desktop.wait.show(1 / 5, "核心组件加载完毕", function () {
            //    desktop.progress.show(2 / 5, "正在获取用户登录信息...", function () {

            //获取用户信息
            ycc.sendJttp("User.GetInfo", {}, function (e) {
                var status = parseInt(e.Header.Status);
                if (status > 0) {
                    //用户已登录则直接进行登录
                    app.info.user = e.Data;
                    loadMain();
                } else {
                    //用户未登录则加载登录界面
                    //app.load(ycc.desktopConfig.UrlEntrance, "/res/Desktop/Login/config.json", $("#dpz_Desktop_Workspace")[0]);
                    desktop.dialog.show(ycc.desktopConfig.UrlEntrance, "/res/Desktop/Login/config.json", {
                        hasHeader: false,
                        hasBackground: false,
                        width: 350,
                        height: 350,
                        style: {
                            left: "unset",
                            right: "300px",
                            marginLeft: "0px",
                            boxShadow: "none",
                            backgroundColor: "#fff",
                            borderRadius: "0px",
                            border: "none"
                        }
                    }, desktop.wait.hide);
                }
            });

            //});
            //});
        });

    };

    //申请或保持交互标识
    if (ycc.session.id !== "") {
        ycc.sendJttp("Session.Keep", null, function (e) {
            var status = parseInt(e.Header.Status);
            if (status <= 0) {
                ycc.sendJttp("Session.Create", null, function (e2) {
                    var status2 = parseInt(e2.Header.Status);
                    if (status2 > 0) {
                        ycc.session.id = e2.Data.Sid;
                        ycc.session.key = e2.Data.Key;
                        localStorage.setItem(ycc.session.storage.id, ycc.session.id);
                        localStorage.setItem(ycc.session.storage.key, ycc.session.key);
                        loadInfoBar();
                    }
                });
            } else {
                loadInfoBar();
            }
        });
    } else {
        ycc.sendJttp("Session.Create", null, function (e) {
            var status = parseInt(e.Header.Status);
            if (status > 0) {
                ycc.session.id = e.Data.Sid;
                ycc.session.key = e.Data.Key;
                localStorage.setItem(ycc.session.storage.id, ycc.session.id);
                localStorage.setItem(ycc.session.storage.key, ycc.session.key);
                loadInfoBar();
            }
        });
    }

    $("#dpz_Desktop_Dialog_Close_Link").click(function () {
        desktop.dialog.close();
    });

    //添加提示框关闭事件
    $("#dpz_Desktop_Delayed_Box").click(function () {
        $("#dpz_Desktop_Delayed").animate({ opacity: 0 }, 300, function () {
            $("#dpz_Desktop_Delayed").css({ display: "none" });
        });
    });

    //改写系统提示框
    window.alert = function (s) {
        desktop.delayed.show(s);
    };

});
