// 添加到页面的window对象上面，在页面中用ys.进行访问
window.signalrProxy = {};
(function ($, signalrProxy) {
    "use strict";
    const token = "QBHNq32VzZK04ou_MnL2Q";
    $.extend(signalrProxy, {
        connection: new signalR.HubConnectionBuilder()
            .withUrl("/chatHub", { accessTokenFactory: () => token }) 
            .build(), 
    });
})(window.jQuery, window.signalrProxy);

signalrProxy.connection.keepAliveIntervalInMilliseconds = 15000; // 默认15000ms 客户端发送心跳包的间隔时间。
signalrProxy.connection.serverTimeoutInMilliseconds = 60000; // 默认30000ms 如果在指定时间内未收到服务器消息，客户端认为

/** 简述
 * 1、超时配置和心跳机制：确保连接稳定性，减少误判断开。
 * 2、传输协议协商：优化传输性能，优先使用 WebSocket。
 * 3、Redis 支持：实现分布式消息广播，适用于大规模部署。
 * 其他配置
 * .withAutomaticReconnect()//自动重连
 * .withHubProtocol(new signalR.JsonHubProtocol())
 * .withTransport(signalR.HttpTransportType.WebSockets) // 仅使用 WebSocket
 */

//开启监听
signalrProxy.connection.start().then(function () {
    //console.log("链接成功"); 
    signalrProxy.connection.invoke("OnlineNotify").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
//接受服务端消息
signalrProxy.connection.on("ReceiveOnlineCount", function (count) {
    //var encodedMsg = "在线人数 : " + count; 
    $("#onlineCount").find("span").html(count);   
});