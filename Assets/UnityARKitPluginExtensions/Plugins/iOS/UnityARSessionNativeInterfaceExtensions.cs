using System.Threading.Tasks;
using UnityEngine.XR.iOS;

namespace UnityARKitPluginExtensions
{
    public static class UnityARSessionNativeInterfaceExtensions
    {
        public static Task<ARWorldMap> GetCurrentWorldMapAsnyc(this UnityARSessionNativeInterface arSessionNativeInterface)
        {
            var t = new TaskCompletionSource<ARWorldMap>();
            arSessionNativeInterface.GetCurrentWorldMapAsync(x => t.TrySetResult(x));
            return t.Task;
        }
    }
}
