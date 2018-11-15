using UniRx;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityMultipeerConnectivity;

public class MultipeerWorldMapReceiver : MonoBehaviour
{
    [SerializeField]
    UnityARCameraManager arCameraManager;

    void Start()
    {
        UnityMCSessionNativeInterface.GetMcSessionNativeInterface()
            .DataReceivedAsObservable<PackableARWorldMap>()
            .Select(x => (ARWorldMap)x)
            .Subscribe(RestartARSessionWith)
            .AddTo(this);
    }

    void RestartARSessionWith(ARWorldMap worldMap)
    {
        if (worldMap != null)
        {
            Debug.LogFormat("Map loaded. Center: {0} Extent: {1}", worldMap.center, worldMap.extent);

            UnityARSessionNativeInterface.ARSessionShouldAttemptRelocalization = true;

            var config = arCameraManager.sessionConfiguration;
            config.worldMap = worldMap;
            UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking;

            Debug.Log("Restarting session with worldMap");
            UnityARSessionNativeInterface.GetARSessionNativeInterface().RunWithConfigAndOptions(config, runOption);
        }
    }
}
