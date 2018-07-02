using System;
using UnityEngine.Networking;

namespace Networking
{
    public class EventPublishableNetworkManager : NetworkManager
    {
        public event EventHandler StoppedServer;

        public override void OnStopServer()
        {
            StoppedServer?.Invoke(this, EventArgs.Empty);
        }
    }
}