using System;
using UnityEngine.Networking;

namespace Networking
{
    public class EventPublishableNetworkDiscovery : NetworkDiscovery
    {
        public event EventHandler<ReceivedBroadcastEventArgs> ReceivedBroadcast;

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            var eventArgs = new ReceivedBroadcastEventArgs(fromAddress, data);
            ReceivedBroadcast?.Invoke(this, eventArgs);
        }
    }
}