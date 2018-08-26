using UniRx.Async;
using UnityEngine.XR.iOS;

namespace UnityARKitPluginExtensions
{
    public static class UnityARSessionNativeInterfaceExtensions
    {
        public static UniTask<ARWorldMap> GetCurrentWorldMapAsnyc(this UnityARSessionNativeInterface arSessionNativeInterface)
        {
            var completionSource = new UniTaskCompletionSource<ARWorldMap>();
            arSessionNativeInterface.GetCurrentWorldMapAsync(worldMap => completionSource.TrySetResult(worldMap));
            return completionSource.Task;
        }
    }
}
