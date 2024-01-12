using HSharp.Util.Extension;
using HSharp.Web.Code;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System; 
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HSharp.Admin.Web.Hubs
{

    public class ChatHub : Hub<IChatClient>
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly OperatorInfo UserInfo;
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
            UserInfo = Operator.Instance.Current().Result;
        }
        /// <summary>
        /// 更新在线人数通知
        /// </summary>
        /// <returns></returns>
        public async Task OnlineNotify()
        { 
            await Clients.All.ReceiveOnlineCount(_connections.Count);
        }

        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            //foreach (var connectionId in _connections.GetConnections(UserInfo?.UserId.ParseToString()))
            //{
            //    await Clients.Client(connectionId).SendMessage(UserInfo?.UserId.ParseToString(), UserInfo.RealName + ": " + message);
            //}

            _logger.LogInformation($"SendMessage===Identity.Name== {UserInfo.RealName} {message}");
 
            await Clients.All.ReceiveMessage(user, message);
        }


        /// <summary>
        /// 向调用客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageCaller(string message)
        {
            _logger.LogInformation($"SendMessageCaller===== {message}");
            await Clients.Caller.ReceiveCaller(message);
        }

        /// <summary>
        /// 客户端连接服务端
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var id = Context.ConnectionId; 

            _connections.Add(UserInfo?.UserId.ParseToString(), id);

            _logger.LogInformation($"客户端ConnectionId=>【{id}】已连接服务器！");

             Clients.All.ReceiveOnlineCount(_connections.Count);

            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;

            _connections.Remove(UserInfo?.UserId.ParseToString(), id);

            _logger.LogInformation($"客户端ConnectionId=>【{id}】已断开服务器连接！");

            Clients.All.ReceiveOnlineCount(_connections.Count);

            return base.OnDisconnectedAsync(exception);
        }
        //public async Task ReceiveLog(object data)
        //{
        //    data = ReadHelper.Read();
        //    await Clients.All.ReceiveLog(data);
        //}
    }
}
