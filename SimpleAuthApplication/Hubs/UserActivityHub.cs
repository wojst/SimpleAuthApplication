using Microsoft.AspNetCore.SignalR;

namespace SimpleAuthApplication.Hubs
{
    public class UserActivityHub : Hub
    {
        public async Task SendUserActivity(string message)
        {
            Console.WriteLine($"Sending user activity: {message}");
            await Clients.All.SendAsync("ReceiveUserActivity", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined.");
        }
    }
}
