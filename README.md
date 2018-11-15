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
            mcSession.SendToAllPeers(worldMap.SerializeToByteArray());
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
        UnityMCSessionNativeInterface.GetMcSessionNativeInterface().DataReceivedEvent += OnDataReceived;
    }

    void OnDataReceived(byte[] data)
    {
        var worldMap = ARWorldMap.SerializeFromByteArray(data);
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

Using [neuecc/MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) as serializer in this example.
If you want to add or modify MessagePackObject, you need pre-code generation on Unity iOS. On macOS, this [issue comment](https://github.com/neuecc/MessagePack-CSharp/pull/155#issuecomment-354580450) is very useful.

Generated code path is `Assets/Scripts/Generated/MessagePackGenerated.cs`

## Requirements

- Unity 2017.1+
  + Examples are dependent on Unity 2018.2+
- iOS 12.0+
- Xcode 10.0+
