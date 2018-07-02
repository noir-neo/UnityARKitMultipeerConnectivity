using System;
using UniRx;

namespace Networking.Extensions
{
    public static class EventPublishableNetworkManagerExtensions
    {
        public static IObservable<Unit> StoppedServerAsObservable(
            this EventPublishableNetworkManager networkManager)
        {
            return Observable.FromEvent<EventHandler, Unit>(
                h => (_, __) => h(Unit.Default),
                h => networkManager.StoppedServer += h,
                h => networkManager.StoppedServer -= h
                );
        }
    }
}
