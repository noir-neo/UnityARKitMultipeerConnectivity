using System;
using UniRx;

namespace UnityEngine.XR.iOS
{

    public partial class UnityARSessionNativeInterface
    {
        public static IObservable<ARUserAnchor> ARUserAnchorAddedAsObservable()
        {
            return Observable.FromEvent<ARUserAnchorAdded, ARUserAnchor>(
                h => a => h(a),
                h => ARUserAnchorAddedEvent += h,
                h => ARUserAnchorAddedEvent -= h
            );
        }
    }
}