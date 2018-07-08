using System;
using AOT;
using UnityEngine.XR.iOS;
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace UnityMultipeerConnectivity
{
    public sealed class UnityMCSessionNativeInterface : IDisposable
    {
        static UnityMCSessionNativeInterface mcSessionNativeInterface;
        public static UnityMCSessionNativeInterface GetMcSessionNativeInterface()
        {
            return mcSessionNativeInterface ?? (mcSessionNativeInterface = new UnityMCSessionNativeInterface());
        }

        public event Action<ARWorldMap> WorldMapReceivedEvent;
        static event Action<ARWorldMap> WorldMapReceivedEventInternal;
        delegate void internal_WorldMapReceived(IntPtr worldMapPtr);

#if !UNITY_EDITOR && UNITY_IOS
        readonly IntPtr nativeMCSession;
#endif

        UnityMCSessionNativeInterface()
        {
            WorldMapReceivedEventInternal += WorldMapReceived;
#if !UNITY_EDITOR && UNITY_IOS
            nativeMCSession = _createNativeMCSession();
            _setCallbacks(nativeMCSession, _world_map_received);
#endif
        }

        public void Dispose()
        {
            WorldMapReceivedEventInternal -= WorldMapReceived;
        }

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        static extern IntPtr _createNativeMCSession();

        [DllImport("__Internal")]
        static extern void _setCallbacks(IntPtr nativeSession, internal_WorldMapReceived worldMapReceivedCallback);

        [DllImport("__Internal")]
        static extern void _sendARWorldMapToAllPeers(IntPtr nativeSession, IntPtr dataPtr);
#endif

        public void SendToAllPeers(IntPtr dataPtr)
        {
#if !UNITY_EDITOR && UNITY_IOS
            _sendARWorldMapToAllPeers(nativeMCSession, dataPtr);
#endif
        }

        [MonoPInvokeCallback(typeof(internal_WorldMapReceived))]
        static void _world_map_received(IntPtr worldMapPtr)
        {
            if (WorldMapReceivedEventInternal == null) return;
            var worldMap = ARWorldMap.FromPtr(worldMapPtr);
            WorldMapReceivedEventInternal(worldMap);
        }

        void WorldMapReceived(ARWorldMap worldMap)
        {
            WorldMapReceivedEvent?.Invoke(worldMap);
        }
    }
}
