using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityMultipeerConnectivity;

public class UserAnchorFactory : MonoBehaviour
{
    [SerializeField]
    ARHitTester arHitTester;

    void Start()
    {
        AnchorReceivedAsObservable()
            .Merge(HitAsObservable())
            .Subscribe(AddAnchor)
            .AddTo(this);
    }

    IObservable<UnityARUserAnchorData> HitAsObservable()
    {
        return arHitTester.HitAsObservable();
    }

    static IObservable<UnityARUserAnchorData> AnchorReceivedAsObservable()
    {
        return UnityMCSessionNativeInterface.GetMcSessionNativeInterface()
            .AnchorReceivedAsObservable();
    }

    static void AddAnchor(UnityARUserAnchorData anchorData)
    {
        UnityARSessionNativeInterface.GetARSessionNativeInterface()
            .AddUserAnchor(anchorData);
    }
}
