/**
 * 大胖子命令管理器
 * */
function DpzCommandsManager() {
    this.items = [];
}

/**
 * 设置命令
 * @param {string} name 命令名称
 * @param {string} description 命令描述
 * @param {void} fn 命令绑定函数
 */
DpzCommandsManager.prototype.set = function (name, description, fn) {
    var list = this.items;
    var idxs = list.length;

    var des = {};
    if (typeof description === "string") {
        des.title = "" + description;
        if (des.title === "") des.title = "无";
    } else if (typeof description === "object") {
        des = description;
    } else if (typeof description === "function") {
        des.title = "无";
        if (dpz.isNull(fn)) {
            fn = description;
        } else {
            throw "参数类型错误";
        }
    } else {
        throw "参数类型错误";
    }

    //by函数接受一个成员名字符串做为参数
    //并返回一个可以用来对包含该成员的对象数组进行排序的比较函数
    var by = function (name) {
        return function (o, p) {
            var a, b;
            if (typeof o === "object" && typeof p === "object" && o && p) {
                a = o[name];
                b = p[name];
                if (a === b) {
                    return 0;
                }
                if (typeof a === typeof b) {
                    return a < b ? -1 : 1;
                }
                return typeof a < typeof b ? -1 : 1;
            }
            else {
                throw "error";
            }
        };
    };

    if (typeof fn === "undefined") {
        if (typeof description === "function") {
            fn = description;
            description = undefined;
        } else {
            throw "缺少函数定义";
        }
    } else {
        if (typeof fn !== "function") {
            throw "缺少函数定义";
        }
    }

    for (var i = 0; i < idxs; i++) {
        if (list[i].name === name) {
            list[i].description = des;
            list[i].handler = fn;
            return;
        }
    }
    list[idxs] = {
        name: name,
        description: des,
        handler: fn
    };
    list.sort(by("Name"));
};

/**
 * 大胖子动态加载器
 * */
function DpzDynamicLoader() {
    this.styles = [];
    this.scripts = [];
    this.images = [];
}

/**
 * 添加一个Css文件
 * @param {string} id 唯一标识符
 * @param {string} url 地址
 */
DpzDynamicLoader.prototype.addCss = function (id, url) {
    var idx = this.Styles.length;
    this.Styles[idx] = { id: id, url: url };
};

/**
 * 添加一个Js文件
 * @param {string} id 唯一标识符
 * @param {string} url 地址
 */
DpzDynamicLoader.prototype.addJs = function (id, url) {
    var idx = this.Scripts.length;
    this.Scripts[idx] = { id: id, url: url };
};

/**
 * 添加一张图片
 * @param {string} url 地址
 */
DpzDynamicLoader.prototype.addImage = function (url) {
    var idx = this.Images.length;
    this.Images[idx] = url;
};

/**
 * 加载一个样式文件
 * @param {string} id  唯一标识符
 * @param {string} url 地址
 * @param {void} fn 回调函数
 */
DpzDynamicLoader.prototype.loadCss = function (id, url, fn) {

    if (url.indexOf("?") > 0) {
        url += "&rnd=" + Math.random();
    } else {
        url += "?rnd=" + Math.random();
    }

    var js = document.getElementById(id);
    if (!js) {
        js = document.createElement("link");
        js.id = id;
        js.href = url;
        js.rel = "stylesheet";

        js.onload = js.onreadystatechange = function () {
            if (!this.readyState || this.readyState === 'loaded' || this.readyState === 'complete') {
                //alert('done');
                if (fn) fn();
            }
            js.onload = js.onreadystatechange = null;
        };

        document.head.appendChild(js);
        //while (!X.Configs.Completed[name]) { }
    } else {
        if (fn) fn();
    }
};

/**
 * 加载一个脚本文件
 * @param {string} id  唯一标识符
 * @param {string} url 地址
 * @param {void} fn 回调函数
 */
DpzDynamicLoader.prototype.loadJs = function (id, url, fn) {
    //var id = "X_" + name
    if (url.indexOf("?") > 0) {
        url += "&rnd=" + Math.random();
    } else {
        url += "?rnd=" + Math.random();
    }

    var js = document.getElementById(id);
    if (!js) {
        js = document.createElement("script");
        js.id = id;
        js.src = url;

        js.onload = js.onreadystatechange = function () {
            if (!this.readyState || this.readyState === 'loaded' || this.readyState === 'complete') {
                //alert('done');
                //执行大胖子专用入口绑定函数
                dpz.initializer.handlersExecute();
                if (fn) fn();
            }
            js.onload = js.onreadystatechange = null;
        };

        document.head.appendChild(js);
        //while (!X.Configs.Completed[name]) { }
    } else {
        if (fn) fn();
    }
};

/**
 * 动态加载图片
 * @param {string} url 地址
 * @param {void} fun 回调函数
 * @param {void} errfun 发生错误时的回调函数
 */
DpzDynamicLoader.prototype.loadImage = function (url, fun, errfun) {
    var Img = new Image();

    Img.onerror = function () {
        if (errfun) errfun({ Url: url, Image: Img });
        if (fun) fun({ Url: url, Image: Img });
    };

    Img.onload = function () {
        if (fun) fun({ Url: url, Image: Img });
    };

    Img.src = url;
};

/**
 * 执行加载
 * @param {void} fn 回调函数
 */
DpzDynamicLoader.prototype.load = function (fn) {

    var my = this;
    var idx = 0;

    //加载所有的样式文件
    var loadStyles = function (fnStyles) {
        if (idx >= my.styles.length) {
            if (typeof fnStyles === "function") fnStyles();
            return;
        }

        //加载样式
        my.loadCss(my.styles[idx].id, my.styles[idx].url, function () {
            idx++;
            loadStyles(fnStyles);
        });
    };

    //加载所有的图片文件
    var loadImages = function (fnImages) {
        if (idx >= my.images.length) {
            if (typeof fnImages === "function") fnImages();
            return;
        }

        //加载样式
        my.loadImage(my.images[idx], function () {
            idx++;
            loadImages(fnImages);
        });
    };

    //加载所有的图片文件
    var loadScripts = function (fnScripts) {
        if (idx >= my.scripts.length) {
            if (typeof fnScripts === "function") fnScripts();
            return;
        }

        //加载样式
        my.loadJs(my.scripts[idx].id, my.scripts[idx].url, function () {
            idx++;
            loadScripts(fnScripts);
        });
    };

    loadStyles(function () {
        idx = 0;
        loadImages(function () {
            idx = 0;
            loadScripts(function () {
                my.styles = [];
                my.images = [];
                my.scripts = [];
                if (typeof fn === "function") fn();
            });
        });
    });
};

/**
* 大胖子软件工作室专用JS开发套件
* */
var dpz = {};
dpz.isArray = function (obj) { return Object.prototype.toString.call(obj) === '[object Array]'; };
dpz.isFunction = function (obj) { return typeof obj === 'function'; };
dpz.isNull = function (obj) {
    switch (typeof obj) {
        case "undefined":
            return true;
        case "object":
            return obj === null;
        default:
            return false;
    }
};

/**
* 大胖子套件信息
* */
dpz.info = {
    name: "D.P.Z Development Kit for Javascript",
    version: "1.1.1901",
    build: 2,
    owner: "大胖子软件工作室",
    getVersion: function () {
        var that = this;
        return that.version + "." + that.build;
    },
    toString: function () {
        var that = this;
        return that.name + " Ver " + that.getVersion();
    }
};

/**
 * 大胖子专用数据操作对象
 * */
dpz.data = {

    /**
     * Json操作对象
     * */
    json: {

        /**
         * 从对象获取标准json字符串
         * @param {object} obj 命令绑定函数
         * @returns {string} 标准json字符串
         * */
        getString: function (obj) {
            var res = "";
            var my = this;
            if (typeof obj === "object") {
                //为对象
                if (dpz.isArray(obj)) {
                    //为数组
                    for (p in obj) {
                        if (typeof obj[p] !== "function") {
                            if (res !== "")
                                res += ",";
                            res += my.getString(obj[p]);
                        }
                    }
                    return "[" + res + "]";
                }
                else {
                    //不为数组
                    for (p in obj) {
                        if (typeof obj[p] !== "function") {
                            if (res !== "")
                                res += ",";
                            res += "\"" + p + "\":";
                            res += my.getString(obj[p]);
                        }
                    }
                    return "{" + res + "}";
                }
            }
            else {
                return "\"" + obj + "\"";
            }
        },

        /**
         * 将标准json字符串转化为
         * @param {string} s 命令绑定函数
         * @returns {object} 标准json字符串
         * */
        parse: function (s) {
            try {
                var obj = eval('(' + s + ')');
                return obj;
            }
            catch (e) {
                console.error("转换对象发生异常!");
                console.error(s);
                return {};
            }
        }
    }
};

/**
 * 大胖子控制台
 * */
dpz.console = {

    lastCommandResult: "",
    lastCommand: "",
    lastCommandScript: "",
    lastErrorCommand: "",
    lastErrorCommandScript: "",

    /**
    * 命令管理器
    * */
    commands: new DpzCommandsManager(),

    /**
     * 执行命令
     * @param {string} cmd 命令字符串
     * @returns {any} 命令返回
     */
    exec: function (cmd) {
        //var arr = dcr.getUtf8Bytes(str);
        var res = "";
        var sz = "" + cmd;
        var c160 = String.fromCharCode(160);
        var c32 = String.fromCharCode(32);
        //var isNote = false;
        var doubleString = false;
        var singleString = false;
        var flag = false;
        var tempString = "";
        var command = "";
        var args = new Array();

        console.log("-> " + sz);

        //遍历字符串
        var chr, idx = 0;
        do {
            chr = sz.charAt(idx);

            if (chr !== "") {
                switch (chr) {
                    case '"':
                        if (flag) {
                            tempString += "\"";
                            flag = false;
                        } else {
                            if (!doubleString) {
                                //当前面还有代码，则不处理该代码
                                //if (tempString != "") {
                                //    return "字符串定义不符合规范";
                                //}
                                tempString += "\"";
                                doubleString = true;
                            } else {
                                //if (command == "") {
                                //    command = tempString;
                                //} else {
                                //    args[args.length] = tempString;
                                //}
                                tempString += "\"";
                                //tempString = "";
                                doubleString = false;
                            }
                        }
                        break;
                    case '\'':
                        if (!doubleString) {
                            if (singleString) {
                                //if (command == "") {
                                //    command = tempString;
                                //} else {
                                //    args[args.length] = tempString;
                                //}
                                tempString += "'";
                                //tempString = "";
                                singleString = false;
                            } else {
                                //if (tempString != "") {
                                //    return "字符串定义不符合规范";
                                //}
                                tempString += "'";
                                singleString = true;
                            }
                        } else {
                            tempString += "'";
                        }
                        break;
                    case '\\':
                        if (flag) {
                            tempString += "\\\\";
                            flag = false;
                        } else {
                            if (doubleString || singleString) {
                                flag = true;
                            }
                        }
                        break;
                    case 'n':
                        if (flag) {
                            tempString += "\\" + chr;
                            flag = false;
                        } else {
                            tempString += chr;
                        }
                        break;
                    case c160:
                    case c32:
                        //空格处理
                        if (!(doubleString || singleString)) {
                            //单词结束
                            if (tempString !== "") {
                                //res += dcrInside.getKeyCode(dcrParse.tempString);
                                if (command === "") {
                                    command = tempString;
                                } else {
                                    args[args.length] = tempString;
                                }
                                tempString = "";
                            }
                            //res += "&nbsp;";
                        } else {
                            tempString += " ";
                        }
                        break;
                    default:
                        if (flag) throw "不支持的转义";
                        tempString += chr;
                        break;
                }
            }
            idx++;

        } while (chr !== "");

        if (tempString !== "") {
            if (command === "") {
                command = tempString;
            } else {
                args[args.length] = tempString;
            }
            tempString = "";
        }

        var idxs = dpz.console.commands.items.length;
        idx = -1;
        var i = 0;
        for (i = 0; i < idxs; i++) {
            if (dpz.console.commands.items[i].name === command) {
                idx = i;
                break;
            }
        }

        var script = "dpz.console.lastCommandResult = dpz.console.commands.items[\"" + idx + "\"].handler(";
        for (i = 0; i < args.length; i++) {
            if (i > 0) script += ",";
            script += args[i];
        }
        script += ")";
        //alert(script);

        if (idx < 0) {
            console.error("Unknow Command \"" + command + "\"");
            script = "NULL";
            dpz.console.lastCommandResult = "未知命令";
            dpz.console.lastErrorCommand = command;
            dpz.console.lastErrorCommandScript = script;
        } else {
            try {
                eval(script);
            } catch (ex) {
                dpz.console.lastCommandResult = ex;
                dpz.console.lastErrorCommand = command;
                dpz.console.lastErrorCommandScript = script;
            }
        }

        if (dpz.console.lastCommandResult === undefined) dpz.console.lastCommandResult = "";

        dpz.console.lastCommand = command;
        dpz.console.lastCommandScript = script;

        return dpz.console.lastCommandResult;
    },

    init: function () {

        var cmds = dpz.console.commands;

        cmds.set("help", "打印所有命令帮助", function () {
            var res = "";
            var idxs = dpz.commands.items.length;
            for (var i = 0; i < idxs; i++) {
                var des = dpz.commands.items[i].description;
                res += dpz.commands.items[i].name;
                if (!dpz.isNull(des.params)) {
                    var args = "";
                    for (var j = 0; j < des.params.length; j++) {
                        var param = "" + des.params[j];
                        if (param !== "") {
                            args += " <" + param + ">";
                        }
                    }
                    res += args;
                }
                res += " - " + des.title;
                if (i < idxs - 1) res += "\n";
            }
            return res;
        });

        cmds.set("log", {
            title: "使用浏览器默认的开发者工具进行调试输出",
            params: ["content"]
        }, function (cnt) { console.log(cnt); });

        cmds.set("get-info", "获取套件信息(返回对象)", function () { return dpz.info; });
        cmds.set("alert", "弹出一个对话框", function (cnt) { alert(cnt); });
        cmds.set("reload", "刷新网页", function () { location.reload(true); });

        cmds.set("goto", {
            title: "跳转网页到新地址",
            params: ["url"]
        }, function (url) {
            if (typeof url !== "string") throw "地址不符合规范";
            location.href = url;
        });

        cmds.set("open", {
            title: "从新的页面打开一个新的地址",
            params: ["url"]
        }, function (url) {
            if (typeof url !== "string") throw "地址不符合规范";
            if (win) win.open(url);
        });

        cmds.set("load-js", {
            title: "动态加载一个js文件",
            params: ["id", "url", "fn"]
        }, dpz.loader.loadJs);

        cmds.set("load-css", {
            title: "动态加载一个css文件",
            params: ["id", "url", "fn"]
        }, dpz.loader.loadCss);

        cmds.set("load-image", {
            title: "动态加载一个css文件",
            params: ["url", "fn", "fnErr"]
        }, dpz.loader.loadImage);

    }
};

/**
 * 大胖子专用动态加载器
 * */
dpz.loader = new DpzDynamicLoader();

/**
 * 大胖子命令管理器
 * */
dpz.commands = dpz.console.commands;

/**
 * 执行大胖子控制台命令
 * @param {string} cmd 命令字符串
 * @returns {any} 命令返回
 */
dpz.exec = function (cmd) {
    return dpz.console.exec(cmd);
};

/**
 * 大胖子套件初始化专用对象
 * */
dpz.initializer = {
    documentIsReady: false,
    domContentLoaded: function () {
        if (typeof document !== "undefined") {
            //取消事件监听，执行ready方法
            if (document.addEventListener) {
                document.removeEventListener("DOMContentLoaded", dpz.initializer.domContentLoaded, false);
                dpz.initializer.documentReady();
            }
            else if (document.readyState === "complete") {
                document.detachEvent("onreadystatechange", dpz.initializer.domContentLoaded);
                dpz.initializer.documentReady();
            }
        }
    },
    documentReady: function () {

        //如果已加载，则退出处理
        if (dpz.initializer.documentIsReady) return;

        //设置已加载标志
        dpz.initializer.documentIsReady = true;
        //tmp.DOMContentLoaded();

        //将dpz加入window对象
        if (typeof window !== "undefined") {
            if (typeof window["dpz"] === "undefined") window.dpz = dpz;
        }

        //进行初始命令设置
        dpz.console.init();

        //执行所有绑定的执行对象
        dpz.initializer.handlersExecute();
    },
    handlers: [],
    handlersExecute: function () {
        var my = this;
        var idx = my.handlers.length;
        for (var i = 0; i < idx; i++) {
            if (typeof my.handlers[i] === "function") my.handlers[i]();
        }
        my.handlers = [];
    }
};

/**
 * 大胖子套件使用入口
 * @param {string} fn 绑定的方法
 * */
dpz.ready = function (fn) {
    var idx = dpz.initializer.handlers.length;
    dpz.initializer.handlers[idx] = fn;
};

//绑定加载事件进行套件初始化
if (typeof document !== "undefined") {
    if (document.addEventListener) {
        // Use the handy event callback
        document.addEventListener("DOMContentLoaded", dpz.initializer.domContentLoaded, false);

        // A fallback to window.onload, that will always work
        if (typeof window !== "undefined") window.addEventListener("load", dpz.initializer.documentReady, false);

        // If IE event model is used
    } else {
        // Ensure firing before onload, maybe late but safe also for iframes
        document.attachEvent("onreadystatechange", dpz.initializer.domContentLoaded);

        // A fallback to window.onload, that will always work
        if (window) window.attachEvent("onload", dpz.initializer.documentReady);
    }
}