var ycc = {};

ycc.version = "1.2.1904.6";

ycc.socket = null;
ycc.socketAutoMessage = true;
ycc.debug = false;

//交互相关
ycc.session = {
    storage: {
        id: "ycc_session_id",
        key: "ycc_session_key"
    },
    id: "",
    key: ""
};

ycc.eventHandlers = [];
ycc.eventHandlersExecute = function (tp, obj) {
    var idx = ycc.eventHandlers.length;
    for (var i = 0; i < idx; i++) {
        if (!dpz.isNull(ycc.eventHandlers[i])) {
            var handler = ycc.eventHandlers[i];
            if (handler.type === tp || handler.type === "*") {
                if (dpz.isFunction(handler.handler)) handler.handler(obj);
                if (!handler.keep) ycc.eventHandlers[i] = null;
            }
        }
    }
};

ycc.readyHandlers = [];
ycc.readyHandlersExecute = function () {
    var idx = ycc.readyHandlers.length;
    for (var i = 0; i < idx; i++) {
        if (dpz.isFunction(ycc.readyHandlers[i])) ycc.readyHandlers[i]();
    }
    ycc.readyHandlers = [];
};

/**
 * 一次性绑定事件
 * @param {any} tp 事件类型
 * @param {any} fn 回调函数
 */
ycc.bindOnce = function (tp, fn) {
    var idx = ycc.eventHandlers.length;
    var obj = {};
    obj.type = tp;
    obj.handler = fn;
    obj.keep = false;
    ycc.eventHandlers[idx] = obj;
};

/**
 * 绑定事件
 * @param {any} tp 事件类型
 * @param {any} fn 回调函数
 */
ycc.bind = function (tp, fn) {
    var idx = ycc.eventHandlers.length;
    var obj = {};
    obj.type = tp;
    obj.handler = fn;
    obj.keep = true;
    ycc.eventHandlers[idx] = obj;
};

ycc.send = function (content) {
    if (dpz.isNull(ycc.socket)) return;

    if (ycc.debug) console.log("WSSend\\>" + content);
    ycc.socket.send(content);
};

ycc.sendJttp = function (tp, data, fn) {
    ycc.bindOnce(tp, fn);
    var obj = {
        Header: {
            Type: tp,
            SessionID: ycc.session.id
        }
    };
    if (!dpz.isNull(data)) obj.Data = data;
    ycc.send(dpz.data.json.getString(obj));
};

ycc.desktopConfig = {
    Description: "",
    Host: "",
    ID: "",
    Name: "",
    Path: "",
    ScriptEntrance: "",
    Text: "",
    UrlEntrance: ""
};

ycc.entityConfig = {
    ID: "",
    Name: "",
    Code: "",
    SecurityKey: "",
    Lv: "",
    DBSign: "",
    InterfaceUrl: "",
    FileSite: "",
    AppSite: "",
    HomeUrl: "",
    SettingUrl: "",
    LogoUrl: "",
    Status: "",
    desktopID: "",
    CreateUserID: "",
    UrlEntrance: "",
    ScriptEntrance: ""
};

/**
 * 核心控制器就绪事件绑定入口
 * @param {any} fn 绑定函数
 */
ycc.ready = function (fn) {
    var idx = ycc.readyHandlers.length;
    ycc.readyHandlers[idx] = fn;
};

ycc.request = {
    getQueryString: function (item) {
        var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
        return svalue ? svalue[1] : svalue;
    }
};

dpz.ready(function () {

    //调试开关
    if (ycc.request.getQueryString("debug") === "1") {
        ycc.debug = true;
    }

    //进行交互标识初始化
    var sid = localStorage.getItem(ycc.session.storage.id);
    if (dpz.isNull(sid)) sid = "";
    ycc.session.id = sid;

    var skey = localStorage.getItem(ycc.session.storage.key);
    if (dpz.isNull(skey)) skey = "";
    ycc.session.key = skey;

    //当桌面加载完毕时加载桌面
    ycc.ready(function () {

        //加载桌面
        var host = window.location.host;
        ycc.sendJttp("Desktop.Get", { Host: host }, function (obj) {
            //console.log(obj);
            ycc.desktopConfig = obj.Data.Row;
            var urlEntrance = ycc.desktopConfig.UrlEntrance;
            var scriptEntrance = ycc.desktopConfig.ScriptEntrance;
            dpz.loader.loadJs("Aos_Js_Desktop", urlEntrance + scriptEntrance);
        });

    });

    var ws = new WebSocket(site.config.websocket.url);

    ycc.socket = ws;

    //连接成功时，触发事件
    ws.onopen = function () {
        //请求参数
        //var param = { "id": 1, "command": "account_info", "account": "r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59" };
        // 使用 send() 方法发送数据
        //ws.send(JSON.stringify(param));
        //alert("数据发送中...");
        //console.log("onopen");

        //调试打印
        console.log("Yunyitong Core Controller is ready!");
        ycc.readyHandlersExecute();
    };

    //接收到服务端响应的数据时，触发事件
    ws.onmessage = function (evt) {
        var data = evt.data;
        if (ycc.debug) console.log("WSMessage\\>" + data);
        var obj = dpz.data.json.parse(data);
        if (!dpz.isNull(obj.Header)) {
            if (ycc.socketAutoMessage) {
                var status = parseInt(obj.Header.Status);
                var msg = "";
                if (!dpz.isNull(obj.Message)) msg += obj.Message;
                if (status >= 0) {
                    if (msg !== "") alert(msg);
                    //执行绑定函数
                    ycc.eventHandlersExecute(obj.Header.Type, obj);
                } else {
                    var err = parseInt(obj.Header.Error);
                    if (msg !== "") alert("交互发生异常:\n# 交互类型: " + obj.Header.Type + "\n# 错误码: 0x" + err.toString(16) + "\n# 提示信息: " + msg);
                }
            } else {
                //执行绑定函数
                ycc.eventHandlersExecute(obj.Header.Type, obj);
            }
        } else {
            console.warn("接收到不支持的数据=>" + data);
        }
        //alert("收到数据..." + data);
        //console.log("onmessage=>" + data);
    };

    ws.onerror = function () {
        console.log("onerror");
    };

    // 断开 web socket 连接成功触发事件
    ws.onclose = function () {
        console.log("onclose");
        alert("与服务器连接已断开,请刷新页面重试!");
    };

    //注册控制台命令
    dpz.commands.set("ycc-send", {
        title: "向服务器发送消息",
        params: ["content"]
    }, ycc.send);

    dpz.commands.set("ycc-send-object", {
        title: "向服务器发送对象封装的消息",
        params: ["object"]
    }, function (obj) {
        ycc.send(dpz.data.json.getString(obj));
    });

    //绑定控制台交互
    ycc.bind("Console", function (obj) {
        //console.log("Console\\>" + obj.Data.Command);
        dpz.exec(obj.Data.Command);
    });

});