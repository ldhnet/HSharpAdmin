 
"use strict";
    var token = "token666"; 
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub", {
        accessTokenFactory: () => token
    }).build(); 
    //Disable send button until connection is established
    /*document.getElementById("sendButton").disabled = true; */
    //connection.on("ReceiveMessage", function (user, message) {
    //    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //    var encodedMsg = user + " says " + msg;
    //    console.log(encodedMsg);
    //    //var li = document.createElement("li");
    //    //li.textContent = encodedMsg;
    //    //document.getElementById("messagesList").appendChild(li);
    //});

    connection.start().then(function () {
        console.log("链接成功");
/*        document.getElementById("sendButton").disabled = false;*/
    }).catch(function (err) {
        return console.error(err.toString());
    });

    //document.getElementById("sendButton").addEventListener("click", function (event) {
    //    var user = document.getElementById("userInput").value;
    //    var message = document.getElementById("messageInput").value;
    //    connection.invoke("SendMessage", user, message).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});
 