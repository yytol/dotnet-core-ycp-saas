var app = {};

//信息
app.info = {
    name: "Black",
    title: "云桌面黑曜石版",
    builds: [
        {
            version: "1.0.1904", build: 1,
            notes: [
                "基于桌面云桌面蓝晶石版(Version 1.1.1904.2)并进行个性化修改",
                "兼容云谊通云平台V5版本核心",
                "移除云桌面蓝晶石版针对账套功能的限制",
                "增加特有页面'首页'设定"
            ]
        },
        {
            version: "1.1.1905", build: 2,
            notes: [
                "将特有页面'首页'设定修改为'概览'",
                "完善消息中心功能界面",
                "修正了当云应用加载失败时，等待界面一直显示的问题",
                "增加了登录界面密码框回车登录的便捷操作"
            ]
        },
        {
            version: "1.1.1905", build: 3,
            notes: [
                "新增SwitchVue云应用类型支持",
                "新增支持多重云应用重载"
            ]
        }
    ],
    getLatestVersion: function () {
        var idx = app.info.builds.length - 1;
        return app.info.builds[idx].version + "." + app.info.builds[idx].build;
    }
};

app.request = {
    getQueryString: function (item) {
        var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
        return svalue ? svalue[1] : svalue;
    }
};

//管理池
app.pool = {
    index: 0,
    getNewIndex: function () {
        app.pool.index++;
        var idx = app.pool.index;
        return idx;
    },
    setPid: function (ele) {
        //var ele = document.createElement("div");
        for (var i = 0; i < ele.children.length; i++) {
            var elec = ele.children[i];
            var idx = "" + this.getNewIndex();
            elec.setAttribute("pid", idx);
            this.setPid(elec);
        }
    },
    setBindEvent: function (ele, obj, parent, index) {
        //var ele = document.createElement("div");
        for (var i = 0; i < ele.children.length; i++) {
            var elec = ele.children[i];
            var idx = "" + this.getNewIndex();
            var pid = "" + elec.getAttribute("pid");
            var bindClick = elec.getAttribute("bind-click");
            if (bindClick !== null) {
                var bindClickArgs = elec.getAttribute("bind-click-args");
                elec.setAttribute("onclick", "app.executeBind('" + pid + "','" + bindClick + "'," + bindClickArgs + ",app.configs." + obj.Name + ",'" + parent + "','" + index + "')");
            }
            //elec.setAttribute("bind-parent", parent);
            //elec.setAttribute("bind-index", index);
            this.setBindEvent(elec, obj, parent, index);
        }
    }
};

//应用配置缓存
app.configs = {};

app.loadHandlers = [];
app.loadHandlersExecute = function (obj) {
    var idx = app.loadHandlers.length;
    for (var i = 0; i < idx; i++) {
        if (dpz.isFunction(app.loadHandlers[i])) app.loadHandlers[i](obj);
    }
    app.loadHandlers = [];
};

/**
 * 绑定应用载入函数
 * @param {void} fn 绑定函数
 */
app.onLoad = function (fn) {
    var idx = app.loadHandlers.length;
    app.loadHandlers[idx] = fn;
};

app.loading = function (fn) {
    var idx = app.loadHandlers.length;
    app.loadHandlers[idx] = fn;
};

app.readyHandlers = [];
app.readyHandlersExecute = function (obj) {
    var idx = app.readyHandlers.length;
    for (var i = 0; i < idx; i++) {
        if (dpz.isFunction(app.readyHandlers[i])) app.readyHandlers[i](obj);
    }
    app.readyHandlers = [];
};

/**
 * 获取绑定图像地址
 * @param {any} obj 配置对象
 * @param {string} key 图像索引键
 * @returns {string} 图像地址
 */
app.getImageSrc = function (obj, key) {
    var idx = obj.Images.length;
    for (var i = 0; i < idx; i++) {
        if (obj.Images[i].Key === key) return obj.Images[i].Src;
    }
    return "";
};

/**
 * 绑定应用就绪函数
 * @param {void} fn 绑定函数
 */
app.ready = function (fn) {
    var idx = app.readyHandlers.length;
    app.readyHandlers[idx] = fn;
};

/**
 * 输出参数变量
 * @param {string} s 待输出字符串
 * @param {object} o 待输出字符串
 * @param {string} key 父对象关键字
 * @returns {string} 输出字符串
 * */
app.renderVars = function (s, o, key) {
    var res = s;
    for (var k in o) {
        if (typeof o[k] === "object") {
            res = app.renderVars(res, o[k], key + k + ".");
        } else {
            res = res.replace(new RegExp("{{" + key + k + "}}", 'g'), o[k]);
        }
    }
    return res;
};

//执行绑定事件
app.executeBind = function (pid, fn, args, obj, parent, idx) {
    var data = {};
    if (parent !== "" && idx >= 0) {
        data = obj[parent][idx];
    }
    obj[fn]({
        pid: pid,
        data: data,
        args: args
    });
};

//Vue管理器
app.vues = {};

app.load = function (entrance, url, parent, args, fn, fnerror) {
    //var cfg = $.getJSON(entrance + url);
    $.ajax({
        url: entrance + url,
        type: "GET",
        datatype: "text",
        success: function (data) {
            //console.log(data);

            //新建一个Vue处理对象
            var obj = {};

            obj.el = parent === "" ? document.body : parent;
            obj.data = dpz.data.json.parse(data);
            app.configs[obj.data.Name] = obj;

            //设置传递参数
            obj.data.Args = args;

            //设置完整图片路径
            var loadImages = function (o) {
                for (var key in o) {
                    if (typeof o[key] === "object") {
                        loadImages(o[key]);
                    } else {
                        var imgSrc = "" + o[key];
                        if (imgSrc.startsWith("/")) {
                            imgSrc = entrance + imgSrc;
                            o[key] = imgSrc;
                        }
                        dpz.loader.addImage(imgSrc);
                    }
                }
            };
            //console.log(obj);
            switch (obj.data.Type) {
                case "Native"://使用原生方式渲染内容
                    $.get(entrance + obj.data.Page, function (html) {
                        obj.InnerHtml = html;

                        //加载所有图片
                        loadImages(obj.data.Images);

                        dpz.loader.addCss("Aos_Css_" + obj.data.Name, entrance + obj.data.Style);
                        dpz.loader.addJs("Aos_Js_" + obj.data.Name, entrance + obj.data.Script);
                        dpz.loader.load(function () {

                            //触发载入事件
                            app.loadHandlersExecute(obj);

                            ////对象刷新事件
                            //obj.refresh = function () {

                            //    //console.log(obj);

                            //    //进行参数填充
                            //    var htmlOutput = "" + obj.InnerHtml;

                            //    //处理所有的块
                            //    var parser = new DOMParser();
                            //    var xmlDoc = parser.parseFromString("<?xml version=\"1.0\" encoding=\"utf-8\" ?><body>" + htmlOutput + "</body>", "text/xml");
                            //    var ele = xmlDoc.getElementsByTagName("body")[0];
                            //    //设置Pid
                            //    app.pool.setPid(ele);
                            //    app.pool.setBindEvent(ele, obj);
                            //    //ele.innerHTML = htmlOutput;
                            //    var blocks = ele.getElementsByTagName("block");
                            //    for (var i = 0; i < blocks.length; i++) {
                            //        var blk = blocks[i];
                            //        var tp = "" + blk.getAttribute("type");
                            //        var bind = "" + blk.getAttribute("bind");
                            //        if (tp === "list") {
                            //            if (bind.startsWith("{") && bind.endsWith("}")) {
                            //                var objBindKey = bind.substr(1, bind.length - 2);
                            //                var objBlk = obj[objBindKey];
                            //                var objHtmlRes = "";
                            //                if (dpz.isArray(objBlk)) {
                            //                    for (var j = 0; j < objBlk.length; j++) {
                            //                        app.pool.setPid(blk);
                            //                        app.pool.setBindEvent(blk, obj, objBindKey, j);
                            //                        var objHtml = blk.innerHTML;
                            //                        var objBind = objBlk[j];
                            //                        objHtmlRes += app.renderVars(objHtml, objBind, "");
                            //                    }
                            //                    blk.outerHTML = objHtmlRes;
                            //                } else {
                            //                    throw bind + " is not an array ver.";
                            //                }
                            //            } else {
                            //                throw bind + " is not an allow ver.";
                            //            }
                            //        }
                            //    }

                            //    htmlOutput = ele.innerHTML;
                            //    htmlOutput = app.renderVars(htmlOutput, obj, "");
                            //    htmlOutput = htmlOutput.replace(/{{}/g, "{").replace(/{}}/g, "}");
                            html = app.renderVars(html, obj.data, "");

                            //填充内容
                            if (parent === "") {
                                $(document.body).html(html);
                            } else {
                                $(parent).html(html);
                            }

                            //    xmlDoc = null;

                            //};

                            //obj.refresh();

                            //触发就绪事件
                            app.readyHandlersExecute(obj);

                            if (dpz.isFunction(fn)) fn(obj);

                        });
                    });
                    break;
                case "Vue"://使用Vue方式渲染内容
                    var vueTarget = "";
                    if (!dpz.isNull(obj.data.Vue)) vueTarget = obj.data.Vue.Target;
                    if (vueTarget === "") vueTarget = obj.data.Target;

                    if (typeof vueTarget === "undefined") throw "Vue Target is undefined";
                    if (vueTarget === "") throw "Vue Target is undefined";

                    //销毁已有Vue管理对象
                    if (!dpz.isNull(app.vues[vueTarget])) {
                        app.vues[vueTarget].$destroy();
                        app.vues[vueTarget] = null;
                    }

                    $.ajax({
                        url: entrance + obj.data.Page,
                        type: "GET",
                        datatype: "text",
                        success: function (html) {
                            obj.InnerHtml = html;

                            //加载所有图片
                            loadImages(obj.data.Images);

                            dpz.loader.addCss("Aos_Css_" + obj.data.Name, entrance + obj.data.Style);
                            dpz.loader.addJs("Aos_Js_" + obj.data.Name, entrance + obj.data.Script);
                            dpz.loader.load(function () {

                                //触发载入事件
                                app.loadHandlersExecute(obj);

                                //填充内容
                                if (parent === "") {
                                    $(document.body).html(html);
                                } else {
                                    $(parent).html(html);
                                }

                                //触发就绪事件
                                app.readyHandlersExecute(obj);

                                //进行函数回调
                                //if (dpz.isFunction(fn)) fn(obj);
                                app.vues[vueTarget] = new Vue(obj);

                                if (dpz.isFunction(fn)) fn(obj);

                            });
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("加载页面失败！<br />Info:" + textStatus + " " + errorThrown);
                            if (dpz.isFunction(fnerror)) fnerror(textStatus, errorThrown);
                        }
                    });

                    break;
                case "SwitchVue"://使用切换式Vue方式渲染内容
                    vueTarget = "SwitchVue";

                    //销毁已有Vue管理对象
                    if (!dpz.isNull(app.vues[vueTarget])) {
                        app.vues[vueTarget].$destroy();
                        app.vues[vueTarget] = null;
                    }

                    $.ajax({
                        url: entrance + obj.data.Page,
                        type: "GET",
                        datatype: "text",
                        success: function (html) {
                            obj.InnerHtml = html;

                            //加载所有图片
                            loadImages(obj.data.Images);

                            dpz.loader.addCss("Aos_Css_" + obj.data.Name, entrance + obj.data.Style);
                            dpz.loader.addJs("Aos_Js_" + obj.data.Name, entrance + obj.data.Script);
                            dpz.loader.load(function () {

                                //触发载入事件
                                app.loadHandlersExecute(obj);

                                //填充内容
                                if (parent === "") {
                                    $(document.body).html(html);
                                } else {
                                    $(parent).html(html);
                                }

                                //触发就绪事件
                                app.readyHandlersExecute(obj);

                                //进行函数回调
                                //if (dpz.isFunction(fn)) fn(obj);
                                app.vues[vueTarget] = new Vue(obj);

                                if (dpz.isFunction(fn)) fn(obj);

                            });
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("加载页面失败！<br />Info:" + textStatus + " " + errorThrown);
                            if (dpz.isFunction(fnerror)) fnerror(textStatus, errorThrown);
                        }
                    });

                    break;
                default: throw "不支持的云桌面类型:" + obj.data.Type;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("提交失败！Info:" + textStatus + " " + errorThrown);
            if (dpz.isFunction(fnerror)) fnerror(textStatus, errorThrown);
        }
    });
};

app.unloadSwitchPage = function (entrance, list, idx, fn) {
    if (idx >= list.length) {
        if (dpz.isFunction(fn)) fn();
        return;
    }

    app.unload(entrance, list[idx].Config, function () {
        app.unloadSwitchPage(entrance, list, idx + 1, fn);
    });
};

app.unload = function (entrance, url, fn) {
    //var cfg = $.getJSON(entrance + url);
    $.ajax({
        url: entrance + url,
        type: "GET",
        datatype: "text",
        success: function (data) {
            //console.log(data);
            var obj = dpz.data.json.parse(data);
            //console.log(obj);
            switch (obj.Type) {
                case "Native":
                case "Vue":
                    var cssNode = document.getElementById("Aos_Css_" + obj.Name);
                    if (cssNode) cssNode.remove();
                    var jsNode = document.getElementById("Aos_Js_" + obj.Name);
                    if (jsNode) jsNode.remove();
                    if (dpz.isFunction(fn)) fn();
                    break;
                case "SwitchVue":
                    cssNode = document.getElementById("Aos_Css_" + obj.Name);
                    if (cssNode) cssNode.remove();
                    jsNode = document.getElementById("Aos_Js_" + obj.Name);
                    if (jsNode) jsNode.remove();
                    app.unloadSwitchPage(entrance, obj.Menu, 0, fn);
                    //if (dpz.isFunction(fn)) fn();
                    break;
                default: throw "不支持的云桌面类型:" + obj.Type;
            }
            app.configs[obj.Name] = undefined;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("提交失败！Info:" + textStatus + errorThrown);
        }
    });
};

dpz.ready(function () {
    console.log("BlueDesktop is loaded!");

    //添加桌面专用命令

    var cfg = ycc.desktopConfig;
    app.load(cfg.UrlEntrance, "/res/Desktop/Index/config.json", "", {});
});