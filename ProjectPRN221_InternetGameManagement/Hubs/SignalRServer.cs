using Microsoft.AspNetCore.SignalR;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class SignalRServer : Hub
    {
        public async Task UpdateTimer(int userId, int remainingMinutes)
        {
            await Clients.All.SendAsync("ReceiveTimerUpdate", userId, remainingMinutes);
        }

        private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name;
            if (username != null && !UserConnections.ContainsKey(username))
            {
                UserConnections.Add(username, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User?.Identity?.Name;
            if (username != null && UserConnections.ContainsKey(username))
            {
                UserConnections.Remove(username);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAdmin(string message)
        {
            if (UserConnections.TryGetValue("Admin", out var adminConnectionId))
            {
                var user = Context.User?.Identity?.Name;
                if (user != null)
                {
                    await Clients.Client(adminConnectionId).SendAsync("ReceiveMessage", user, message);
                }
            }
        }

        public async Task SendMessageToUser(string user, string message)
        {
            if (UserConnections.TryGetValue(user, out var userConnectionId))
            {
                await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", "admin", message);
            }
        }
    }
}
