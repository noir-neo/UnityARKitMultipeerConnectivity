using System;
using UniRx;

namespace Networking.Extensions
{
    public static class EventPublishableNetworkDiscoveryExtensions
    {
        public static IObservable<ReceivedBroadcastEventArgs> ReceivedBroadcastAsObservable(
            this EventPublishableNetworkDiscovery networkDiscovery)
        {
            return Observable.FromEvent<EventHandler<ReceivedBroadcastEventArgs>, ReceivedBroadcastEventArgs>(
                h => (_, args) => h(args),
                h => networkDiscovery.ReceivedBroadcast += h,
                h => networkDiscovery.ReceivedBroadcast -= h
                );
        }
    }
}
