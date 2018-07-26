using UniRx.Async;
using UnityEngine.XR.iOS;

namespace UnityARKitPluginExtensions
{
    public static class UnityARSessionNativeInterfaceExtensions
    {
        public static IPromise<ARWorldMap> GetCurrentWorldMapPromise(this UnityARSessionNativeInterface arSessionNativeInterface)
        {
            return new Promise<ARWorldMap>((resolve, _) =>
                arSessionNativeInterface.GetCurrentWorldMapAsync(resolve.SetResult));
        }

        public static UniTask<ARWorldMap> GetCurrentWorldMapAsnyc(this UnityARSessionNativeInterface arSessionNativeInterface)
        {
            var promise = arSessionNativeInterface.GetCurrentWorldMapPromise();
            return new UniTask<ARWorldMap>(promise);
        }
    }
}
