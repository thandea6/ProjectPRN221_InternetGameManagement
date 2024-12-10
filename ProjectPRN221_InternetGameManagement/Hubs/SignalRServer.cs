using Microsoft.AspNetCore.SignalR;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class SignalRServer: Hub
    {
        public async Task UpdateTimer(int userId, int remainingMinutes)
        {
            await Clients.All.SendAsync("ReceiveTimerUpdate", userId, remainingMinutes);
        }

    }
}
