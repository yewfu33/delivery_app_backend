using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Hubs
{
    public class TrackingHub : Hub
    {
        // id storage
        private static Hashtable clientIdCollection = new Hashtable();

        // function interpolate courier location to client
        public async Task LatLngToServer(double latitude, double longitude, int clientId)
        {
            if(clientIdCollection.ContainsKey(clientId))
            {
                await Clients.Client(clientIdCollection[clientId].ToString())
                    .SendAsync("CourierLocation", latitude, longitude);
            }
        }

        // override the onConnected callback
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("OnHubConnected", "Hub connected");
        }

        // register the context id and client id into the hashtable storage
        public void registerConnectionId(int uid)
        {
            if (clientIdCollection.ContainsKey(uid))
                clientIdCollection[uid] = Context.ConnectionId;
            else
                clientIdCollection.Add(uid, Context.ConnectionId);
        }

        // return the connection id
        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
            // fix later on
            if (clientIdCollection.ContainsKey(Context.ConnectionId))
                clientIdCollection.Remove(Context.ConnectionId);
        }
    }
}
