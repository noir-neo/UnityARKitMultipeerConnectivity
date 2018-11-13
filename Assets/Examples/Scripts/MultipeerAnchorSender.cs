using UniRx;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityMultipeerConnectivity;

public class MultipeerAnchorSender : MonoBehaviour
{
    [SerializeField]
    ARHitTester arHitTester;

    void Start()
    {
        arHitTester.HitAsObservable()
            .Subscribe(SendAnchorToAllPeers)
            .AddTo(this);
    }

    static void SendAnchorToAllPeers(UnityARUserAnchorData anchorData)
    {
        UnityMCSessionNativeInterface.GetMcSessionNativeInterface()
            .SendToAllPeers((PackableARUserAnchor)anchorData);
    }
}
