using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class OrderHub : Hub
    {
        // Hàm gửi thông báo tới tất cả client
        public async Task NotifyOrder(string username, object[] products)
        {
            // Lấy thời gian hiện tại
            string time = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");

            // Gửi thông báo tới tất cả client
            await Clients.All.SendAsync("ReceiveOrderNotification", username, products, time);
        }
    }
}
