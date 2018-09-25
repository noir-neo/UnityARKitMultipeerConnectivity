using System;
using UniRx;
using UnityEngine.XR.iOS;

namespace UnityMultipeerConnectivity
{
    public static class UnityMCSessionNativeInterfaceExtensions
    {
        public static IObservable<ARWorldMap> WorldMapReceivedAsObservable(this UnityMCSessionNativeInterface mcSessionNativeInterface)
        {
            return Observable.FromEvent<ARWorldMap>(
                h => mcSessionNativeInterface.WorldMapReceivedEvent += h,
                h => mcSessionNativeInterface.WorldMapReceivedEvent -= h
            );
        }

        public static IObservable<UnityARUserAnchorData> AnchorReceivedAsObservable(
            this UnityMCSessionNativeInterface mcSessionNativeInterface)
        {
            return Observable.FromEvent<UnityARUserAnchorData>(
                h => mcSessionNativeInterface.AnchorReceivedEvent += h,
                h => mcSessionNativeInterface.AnchorReceivedEvent -= h
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
