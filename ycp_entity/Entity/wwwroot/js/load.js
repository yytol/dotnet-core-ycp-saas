/// <reference path="../lib/jquery/dist/jquery.js" />
/// <reference path="../lib/dpz/dpz.js" />
/// <reference path="../lib/ycp/ycc/ycc.js" />
/// <reference path="../lib/ycp/desktops/blue/app/load.js" />
/// <reference path="../lib/ycp/desktops/blue/desktop/page.js" />

dpz.ready(function () {
    //alert("OK");

    var loadDefautApp = function (r) {
        setTimeout(function () {
            desktop.body.show(ycc.entityConfig.UrlEntrance, r["Path"] + "config.json");
        }, 10);
    };

    //加载黑曜石桌面特有概览界面
    var loadOverviewApp = function (r) {
        setTimeout(function () {
            desktop.body.show(ycc.entityConfig.UrlEntrance, "/res/Overview/Home/config.json");
        }, 10);
    };

    //获取用户信息
    var getAppList = function () {

        ycc.sendJttp("App.GetList", { AuthID: ycc.entityConfig.ID }, function (e) {
            var status = parseInt(e.Header.Status);
            if (status > 0) {
                //console.log(e.Data);
                var ls = e.Data.List;
                var r = null;
                switch (app.info.name) {
                    case "Black"://兼容黑曜石版桌面
                        var menuHtml = "";
                        for (var i = 0; i < ls.length; i++) {
                            r = ls[i];
                            menuHtml += "<a href=\"javascript:;\" onclick=\"desktop.body.show('" + ycc.entityConfig.UrlEntrance + "', '" + r["Path"] + "config.json');desktop.closeMenu();\" title=\"" + r["Text"] + "\">";
                            menuHtml += "<i><img src=\"" + ycc.entityConfig.UrlEntrance + r["Path"] + "/logo/black.png\"/></i>";
                            menuHtml += "<s>" + r["Text"] + "</s>";
                            menuHtml += "</a>";
                            //if (i === 0) loadDefautApp(r);
                        }
                        menuHtml += "</dl>";
                        $("#dpz_Desktop_Main_MenuList").html(menuHtml);

                        //黑曜石桌面默认进概览
                        loadOverviewApp();

                        break;
                    case "Blue"://兼容蓝晶石版桌面
                        menuHtml = "<dl>";
                        for (i = 0; i < ls.length; i++) {
                            r = ls[i];
                            menuHtml += "<dt><img src=\"" + ycc.entityConfig.UrlEntrance + r["Path"] + "/logo/blue.png\"></dt>";
                            menuHtml += "<dd><a href=\"javascript:;\" onclick=\"desktop.body.show('" + ycc.entityConfig.UrlEntrance + "', '" + r["Path"] + "config.json');\">" + r["Text"] + "</a></dd>";
                            if (i === 0) loadDefautApp(r);
                        }
                        menuHtml += "</dl>";
                        $("#dpz_Desktop_Main_MenuList").html(menuHtml);
                        break;
                    default:
                        alert("不支持的云桌面版本\"" + app.info.name + "\"");
                        break;
                }
                desktop.wait.hide();
                //console.log(ycc.entityConfig);
                //dpz.loader.loadJs("Aos_Js_Entitiy", ycc.entityConfig.UrlEntrance + ycc.entityConfig.ScriptEntrance);
                //desktop.progress.show(5 / 5, "加载完成!", function () {
                //    desktop.progress.hide();
                //});
            }
        });
    };

    desktop.wait.show("正在获取应用列表");

    var userName = app.info.user.Name;
    var str = "weifhewvsavafqehfor32hior2wfwgorehvsoqfefheofewbqf32fowevbewvbwe";
    var key = "name=" + userName + "&str=" + str + "&key=" + ycc.session.key;
    var md5 = dpz.security.getMD5(key);

    var times = 0;
    var getUserInfo = function () {
        $.ajax({
            url: ycc.entityConfig.UrlEntrance + "/Api/Ycc/GetUserInfo",
            data: { sid: ycc.session.id, str: str, md5: md5 },
            type: "GET",
            datatype: "text",
            success: function (e) {
                var obj = dpz.data.json.parse(e);
                //console.log(obj);
                var status = parseInt(obj.Header.Status);
                if (status > 0) {
                    ycc.entityConfig.SessionID = obj.Header.SessionID;
                    getAppList();
                } else {
                    desktop.wait.hide();
                    alert(obj.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                times++;
                if (times <= 5) {
                    desktop.wait.show("获取发生异常，准备第" + times + "次重试");
                    setTimeout(getUserInfo, 1000);
                } else {
                    desktop.wait.hide();
                    alert("获取失败,请刷新重试！Info:" + textStatus + errorThrown);
                }

            }
        });
    };

    //获取用户信息
    getUserInfo();


});