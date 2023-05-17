using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System; 
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Hubs
{

    public class ChatHub : Hub<IChatClient>
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            //_logger.LogInformation($"SendMessage===== {user} {message}");
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
            _logger.LogInformation($"客户端ConnectionId=>【{id}】已连接服务器！");
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
            _logger.LogInformation($"客户端ConnectionId=>【{id}】已断开服务器连接！");
            return base.OnDisconnectedAsync(exception);
        }
        //public async Task ReceiveLog(object data)
        //{
        //    data = ReadHelper.Read();
        //    await Clients.All.ReceiveLog(data);
        //}
    }
}
