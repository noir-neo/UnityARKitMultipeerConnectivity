using System;
using MessagePack;
using UniRx;

namespace UnityMultipeerConnectivity
{
    public static class UnityMCSessionNativeInterfaceExtensions
    {
        public static void SendToAllPeers<T>(this UnityMCSessionNativeInterface mcSessionNativeInterface, T data)
            where T : IMessagePackUnion
        {
            var bin = MessagePackSerializer.Serialize<IMessagePackUnion>(data);
            mcSessionNativeInterface.SendToAllPeers(bin);
        }

        public static IObservable<T> DataReceivedAsObservable<T>(this UnityMCSessionNativeInterface mcSessionNativeInterface)
            where T : IMessagePackUnion
        {
            return mcSessionNativeInterface.DataReceivedAsObservable()
                .Select(MessagePackSerializer.Deserialize<IMessagePackUnion>)
                .OfType<IMessagePackUnion, T>();
        }

        public static IObservable<byte[]> DataReceivedAsObservable(this UnityMCSessionNativeInterface mcSessionNativeInterface)
        {
            return Observable.FromEvent<byte[]>(
                h => mcSessionNativeInterface.DataReceivedEvent += h,
                h => mcSessionNativeInterface.DataReceivedEvent -= h
                );
        }

        public static IObservable<Tuple<UnityMCPeerID, UnityMCSessionState>> StateChangedAsObservable(
            this UnityMCSessionNativeInterface mcSessionNativeInterface)
        {
            return Observable.FromEvent<Action<UnityMCPeerID, UnityMCSessionState>, Tuple<UnityMCPeerID, UnityMCSessionState>>(
                h => (peerID, state) => h(Tuple.Create(peerID, state)),
                h => mcSessionNativeInterface.StateChangedEvent += h,
                h => mcSessionNativeInterface.StateChangedEvent -= h
            );
        }
    }
}
