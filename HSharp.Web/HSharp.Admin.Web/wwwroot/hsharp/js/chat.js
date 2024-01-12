 
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


// 添加到页面的window对象上面，在页面中用ys.进行访问
; window.signalrProxy = {};
(function ($, signalrProxy) {
    "use strict";
     const token = "token666";

    $.extend(signalrProxy, {
        connection: new signalR.HubConnectionBuilder().withUrl("/chatHub", { accessTokenFactory: () => token }).build(), 
    });
})(window.jQuery, window.signalrProxy);
  
signalrProxy.connection.start().then(function () {
    console.log("链接成功"); 
    signalrProxy.connection.invoke("OnlineNotify").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});


signalrProxy.connection.on("ReceiveOnlineCount", function (count) {

    var encodedMsg = "在线人数 : " + count;

    console.log(encodedMsg);

    ys.msgWarning(encodedMsg); 

    $("#onlineCount").find("span").html(count);  
    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
});