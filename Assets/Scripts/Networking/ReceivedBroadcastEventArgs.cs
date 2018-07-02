using System;

namespace Networking
{
    public class ReceivedBroadcastEventArgs : EventArgs
    {
        public string FromAddress { get; }
        public string Data { get; }

        public ReceivedBroadcastEventArgs(string fromAddress, string data)
        {
            FromAddress = fromAddress;
            Data = data;
        }
    }
}