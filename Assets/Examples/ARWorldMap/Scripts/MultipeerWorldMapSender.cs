using System;
using System.Threading;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityMultipeerConnectivity;
using ARWorldMap = UnityEngine.XR.ARKit.ARWorldMap;

public class MultipeerWorldMapSender : MonoBehaviour
{
    [SerializeField] Button sendWorldMapButton;

    [SerializeField] ARSession arSession;

    void Start()
    {
        sendWorldMapButton.OnClickAsObservable()
            .Select(_ => GetARWorldMapAsync().ToObservable())
            .Switch()
            .Select(x => (PackableARWorldMap)x)
            .Subscribe(UnityMCSessionNativeInterface.GetMcSessionNativeInterface().SendToAllPeers)
            .AddTo(this);
    }

    async UniTask<ARWorldMap> GetARWorldMapAsync()
    {
        if (!(arSession.subsystem is ARKitSessionSubsystem arKitSessionSubsystem))
        {
            throw new Exception("No session subsystem available. Could not load.");
        }
        return await arKitSessionSubsystem.GetARWorldMapTask();
    }
}

public static class ARKitSessionSubsystemExtensions
{
    public static async UniTask<ARWorldMap> GetARWorldMapTask(this ARKitSessionSubsystem session, CancellationToken cancellationToken = default)
    {
        using (var request = session.GetARWorldMapAsync())
        {
            while (!request.status.IsDone())
            {
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (request.status.IsError())
            {
                throw new Exception($"Session getting AR world map failed with status {request.status}");
            }

            return request.GetWorldMap();
        }
    }
}