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
    }
}
