using System;
using System.Threading;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityMultipeerConnectivity;

namespace UnityARKitMultipeerConnectivity.Examples.CollaborativeSessions
{
    public class MultipeerCollaborationDataSender : MonoBehaviour
    {
        [SerializeField]
        ARSession arSession;

        void Start()
        {
            if (!ARKitSessionSubsystem.supportsCollaboration)
                return;

            if (arSession == null)
                return;

            if (!(arSession.subsystem is ARKitSessionSubsystem arKitSessionSubsystem))
            {
                throw new Exception("No session subsystem available. Could not load.");
            }

            arKitSessionSubsystem.collaborationEnabled = true;

            var mcSession = UnityMCSessionNativeInterface.GetMcSessionNativeInterface();

            SendCollaborationDataAsync(arKitSessionSubsystem, mcSession, this.GetCancellationTokenOnDestroy()).Forget();
        }

        static async UniTask SendCollaborationDataAsync(ARKitSessionSubsystem arKitSessionSubsystem, UnityMCSessionNativeInterface mcSession, CancellationToken cancellationToken)
        {
            while (true)
            {
                while (0 < arKitSessionSubsystem.collaborationDataCount)
                {
                    var data = arKitSessionSubsystem.DequeueCollaborationData();
                    if (!data.valid) continue;
                    mcSession.SendToAllPeers((PackableARCollaborationData) data);
                }
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}