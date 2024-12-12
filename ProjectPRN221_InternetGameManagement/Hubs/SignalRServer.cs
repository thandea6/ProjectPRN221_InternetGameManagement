using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Hubs
{
    public class SignalRServer : Hub
    {
        public async Task SendMessageToServer(string user, string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(message))
                {
                    // Broadcast the message to all connected clients
                    await Clients.All.SendAsync("ReceiveMessage", user, message);
                }
                else
                {
                    // Log an error or handle the case where user or message is null/empty
                    throw new ArgumentException("User or message is missing.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception with more details
                Console.WriteLine($"Error in SendMessageToAdmin: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
