# UnityARKitMultipeerConnectivity

Using the MultipeerConnectivity framework to share AR world between nearby devices.

This is a reimplementation of [Apple's sample code](https://developer.apple.com/documentation/arkit/creating_a_multiuser_ar_experience) with Unity.

## Usage

1. From Source
    - Clone or download and copy `Assets/UnityMultipeerConnectivity`, `Assets/UnitySwift` (and `Assets/UnityARKitPlugin`) directories to your own project.

## Example code

### Send ARWorldMap

```ARWorldMapSender.cs
public static class ARWorldMapSender
{
    public static void SendARWorld()
    {
        var mcSession = UnityMCSessionNativeInterface.GetMcSessionNativeInterface();
        var arSession = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        arSession.GetCurrentWorldMapAsync(worldMap => {
            mcSession.SendToAllPeers(worldMap.nativePtr);
        });
    }
}
```

### Receive ARWorldMap and relocalize

```ARWorldMapReceiver.cs
public class ARWorldMapReceiver : MonoBehaviour
{
    [SerializeField] UnityARCameraManager arCameraManager;

    void Start()
    {
        UnityMCSessionNativeInterface.GetMcSessionNativeInterface().WorldMapReceivedEvent += Relocalize;
    }

    void Relocalize(ARWorldMap worldMap)
    {
        UnityARSessionNativeInterface.ARSessionShouldAttemptRelocalization = true;
        var config = arCameraManager.sessionConfiguration;
        config.worldMap = worldMap;
        UnityARSessionRunOption runOption =
            UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors |
            UnityARSessionRunOption.ARSessionRunOptionResetTracking;
        UnityARSessionNativeInterface.GetARSessionNativeInterface()
            .RunWithConfigAndOptions(config, runOption);
    }
}
```

### And more

See [Examples](https://github.com/noir-neo/UnityARKitMultipeerConnectivity/tree/master/Assets/Examples).

## Requirements

- Unity 2017.1+
  + Examples are dependent on Unity 2018.2+
- iOS 12.0+
- Xcode 10.0+
