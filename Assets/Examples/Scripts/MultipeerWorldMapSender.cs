using UniRx;
using UniRx.Async;
using UnityARKitPluginExtensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;
using UnityMultipeerConnectivity;

public class MultipeerWorldMapSender : MonoBehaviour
{
    [SerializeField] Button sendWorldMapButton;

    void Start()
    {

        sendWorldMapButton.OnClickAsObservable()
            .Subscribe(async _ => await SendCurrentARWorldMapToAllPeersAsync())
            .AddTo(this);
    }

    static async UniTask SendCurrentARWorldMapToAllPeersAsync()
    {
        var arSessionNativeInterface = UnityARSessionNativeInterface.GetARSessionNativeInterface();
        var arWorldMap = await arSessionNativeInterface.GetCurrentWorldMapAsnyc();
        var nativePtr = arWorldMap.nativePtr;
        var mcSessionNativeInterface = UnityMCSessionNativeInterface.GetMcSessionNativeInterface();
        mcSessionNativeInterface.SendToAllPeers(nativePtr);
    }
}
