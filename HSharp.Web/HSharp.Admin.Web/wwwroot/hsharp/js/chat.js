 
//"use strict";
//    var token = "token666";
//    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub", {
//        accessTokenFactory: () => token
//    }).build();
//    //Disable send button until connection is established
//    /*document.getElementById("sendButton").disabled = true; */
//    connection.on("ReceiveMessage", function (user, message) {
//        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//        var encodedMsg = user + " says " + msg;
//        console.log(encodedMsg);
//        //var li = document.createElement("li");
//        //li.textContent = encodedMsg;
//        //document.getElementById("messagesList").appendChild(li);
//    });

//    connection.start().then(function () {
//        console.log("链接成功");
//    }).catch(function (err) {
//        return console.error(err.toString());
//    });

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    alter(111111);
//        var user = document.getElementById("userInput").value;
//        var message = document.getElementById("messageInput").value;
//        connection.invoke("SendMessage", user, message).catch(function (err) {
//            return console.error(err.toString());
//        });
//        event.preventDefault();
//    });


//// 添加到页面的window对象上面，在页面中用ys.进行访问
//; window.signalrProxy = {};
//(function ($, signalrProxy) {
//    "use strict";
//     const token = "token666";

//    $.extend(signalrProxy, {
//        connection: function () {
//            return new signalR.HubConnectionBuilder().withUrl("/chatHub", { accessTokenFactory: () => token }).build();
//        }, 
//    });
//})(window.jQuery, window.signalrProxy);


"use strict";
// 添加到页面的window对象上面，在页面中用signalrProxy.进行访问
var token = "token666";
window.signalrProxy = {
    connection :  new signalR.HubConnectionBuilder().withUrl("/chatHub", { accessTokenFactory: () => token }).build()
}; 

console.log("layout",signalrProxy);


signalrProxy.connection.start().then(function () {
    console.log("链接成功");
    const user = "张三";
    const message = "登录成功";
    signalrProxy.connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});


signalrProxy.connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    console.log(encodedMsg);

    var badgeHtml = $('<span class="badge label-danger">' + encodedMsg + '</span>');
    $("#sysOnline").find("i").html(badgeHtml);  


    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
});