using System;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityMultipeerConnectivity;
using ARWorldMap = UnityEngine.XR.ARKit.ARWorldMap;

public class MultipeerWorldMapReceiver : MonoBehaviour
{
    [SerializeField]
    ARSession arSession;

    void Start()
    {
        UnityMCSessionNativeInterface.GetMcSessionNativeInterface()
            .DataReceivedAsObservable<PackableARWorldMap>()
            .Select(x => (ARWorldMap)x)
            .Subscribe(ApplyWorldMap)
            .AddTo(this);
    }

    void ApplyWorldMap(ARWorldMap arWorldMap)
    {
        arSession.Reset();

        if (!(arSession.subsystem is ARKitSessionSubsystem arKitSessionSubsystem))
        {
            throw new Exception("No session subsystem available. Could not load.");
        }
        if (!arWorldMap.valid)
        {
            throw new Exception("Data is not a valid ARWorldMap");
        }
        arKitSessionSubsystem.ApplyWorldMap(arWorldMap);
    }
}
