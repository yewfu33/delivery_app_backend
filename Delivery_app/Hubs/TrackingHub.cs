using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Hubs
{
    public class TrackingHub : Hub
    {
        public Task LatLngToServer(double latitude, double longitude)
        {
            return Clients.All.SendAsync("CouriersLocation", latitude, longitude);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnHubConnected", "Hub connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
        }
    }
}
