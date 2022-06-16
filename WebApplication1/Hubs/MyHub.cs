using Microsoft.AspNetCore.SignalR;
namespace WebApplication1.Hubs
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string user, string contactName, string message)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("RecieveMessage", user, contactName, message);
            }
        }    
        public async Task SendContact(string user, string contactName, string server)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("RecieveContact", user, contactName, server);
            }
        }

    }
}
