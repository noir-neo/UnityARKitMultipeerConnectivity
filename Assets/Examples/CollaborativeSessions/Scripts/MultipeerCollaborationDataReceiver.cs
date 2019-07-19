using System;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityMultipeerConnectivity;

namespace UnityARKitMultipeerConnectivity.Examples.CollaborativeSessions
{
    public class MultipeerCollaborationDataReceiver : MonoBehaviour
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

            void OnReceive(PackableARCollaborationData data)
            {
                var collaborationData = (ARCollaborationData) data;
                if (!collaborationData.valid) return;
                arKitSessionSubsystem.UpdateWithCollaborationData(collaborationData);
            }

            var mcSession = UnityMCSessionNativeInterface.GetMcSessionNativeInterface();

            mcSession.DataReceivedAsObservable<PackableARCollaborationData>()
                .Subscribe(OnReceive)
                .AddTo(this);
        }
    }
}
