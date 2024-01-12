using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks; 

namespace HSharp.Admin.Web.Hubs
{ 
    public class NotifyHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =  new ConnectionMapping<string>();

        public void SendNotifyMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;

            foreach (var connectionId in _connections.GetConnections(who))
            {
                Clients.Client(connectionId).SendAsync(name + ": " + message);
            }
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        
    }
}
