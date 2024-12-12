using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class SignalRServer : Hub
    {
        public async Task UpdateRemainingTime(int userId, int remainingTime)
        {
            // Cập nhật dữ liệu vào database
            var user = await InternetGameManagementContext.Ins.Accounts.FindAsync(userId);
            if (user != null)
            {
                user.Time = remainingTime;  // Cập nhật thời gian còn lại trong database
                await InternetGameManagementContext.Ins.SaveChangesAsync();
            }

        }
    }
}
