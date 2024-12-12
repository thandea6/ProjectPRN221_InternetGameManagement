using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class TimeHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TimeHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task UpdateRemainingTime(int userId, int remainingTime)
        {
            // Cập nhật dữ liệu vào database
            var user = await InternetGameManagementContext.Ins.Accounts.FindAsync(userId);  // Sử dụng context đã tiêm vào
            if (user != null)
            {
                user.Time = remainingTime;  // Cập nhật thời gian còn lại trong database
                await InternetGameManagementContext.Ins.SaveChangesAsync();  // Lưu thay đổi vào database
            }

            // Gửi dữ liệu thời gian còn lại đến tất cả các client (nếu cần thiết)
            await Clients.All.SendAsync("ReceiveRemainingTime", userId, remainingTime);
        }


        public async Task UpdateTime(int userId, int remainingTime)
        {
            Console.WriteLine("UserID LA:" + userId);
            Console.WriteLine("REMAINING TIME LA: "+remainingTime);

            var user = await InternetGameManagementContext.Ins.Accounts.FindAsync(userId);  // Sử dụng context đã tiêm vào
            if (user != null)
            {
                user.Time = remainingTime;  // Cập nhật thời gian còn lại trong database
                await InternetGameManagementContext.Ins.SaveChangesAsync();  // Lưu thay đổi vào database
            }
            Console.WriteLine("DA VAO DUOC HAM UPDATE TIME");
            // Lưu thời gian vào session
            _httpContextAccessor.HttpContext.Session.SetInt32("RemainingTime", remainingTime);

            // Có thể gửi lại thông tin cho tất cả client nếu cần
            await Clients.All.SendAsync("ReceiveTimeUpdate", remainingTime);
        }
    }
}
