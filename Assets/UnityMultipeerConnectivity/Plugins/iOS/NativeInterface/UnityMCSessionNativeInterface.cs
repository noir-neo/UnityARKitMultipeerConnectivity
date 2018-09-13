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

        public event Action<UnityARUserAnchorData> AnchorReceivedEvent;
        static event Action<UnityARUserAnchorData> AnchorReceivedInternal;
        delegate void internal_AnchorReceived(IntPtr anchorPtr);

        public event Action<UnityMCPeerID, UnityMCSessionState> StateChangedEvent;
        static event Action<UnityMCPeerID, UnityMCSessionState> StateChangedInternal;
        delegate void internal_StateChanged(UnityMCPeerID peerId, UnityMCSessionState sessionState);

#if !UNITY_EDITOR && UNITY_IOS
        readonly IntPtr nativeMCSession;
#endif

        UnityMCSessionNativeInterface()
        {
            WorldMapReceivedEventInternal += WorldMapReceived;
            AnchorReceivedInternal += AnchorReceived;
            StateChangedInternal += StateChanged;
#if !UNITY_EDITOR && UNITY_IOS
            nativeMCSession = _createNativeMCSession();
            _setCallbacks(nativeMCSession, _world_map_received, _anchor_received, _state_changed);
#endif
        }

        public void Dispose()
        {
            WorldMapReceivedEventInternal -= WorldMapReceived;
            AnchorReceivedInternal -= AnchorReceived;
            StateChangedInternal -= StateChanged;
        }

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        static extern IntPtr _createNativeMCSession();

        [DllImport("__Internal")]
        static extern void _setCallbacks(IntPtr nativeSession, internal_WorldMapReceived worldMapReceivedCallback, internal_AnchorReceived anchorReceivedCallback,  internal_StateChanged stateChangedCallback);

        [DllImport("__Internal")]
        static extern void _sendARWorldMapToAllPeers(IntPtr nativeSession, IntPtr dataPtr);

        [DllImport("__Internal")]
        static extern void _sendARAnchorToAllPeers(IntPtr nativeSession, UnityARUserAnchorData anchorData);

        [DllImport("__Internal")]
        static extern UnityARUserAnchorData _unityARUserAnchorDataFromARAnchorPtr(IntPtr anchorPtr);
#endif

        public void SendToAllPeers(IntPtr dataPtr)
        {
#if !UNITY_EDITOR && UNITY_IOS
            _sendARWorldMapToAllPeers(nativeMCSession, dataPtr);
#endif
        }

        public void SendToAllPeers(UnityARUserAnchorData anchorData)
        {
#if !UNITY_EDITOR && UNITY_IOS
            _sendARAnchorToAllPeers(nativeMCSession, anchorData);
#endif
        }

        static UnityARUserAnchorData UnityARUserAnchorDataFromARAnchorPtr(IntPtr anchorPtr)
        {
#if !UNITY_EDITOR && UNITY_IOS
            return _unityARUserAnchorDataFromARAnchorPtr(anchorPtr);
#else
            return new UnityARUserAnchorData();
#endif
        }

        [MonoPInvokeCallback(typeof(internal_WorldMapReceived))]
        static void _world_map_received(IntPtr worldMapPtr)
        {
            if (WorldMapReceivedEventInternal == null) return;
            var worldMap = ARWorldMap.FromPtr(worldMapPtr);
            WorldMapReceivedEventInternal(worldMap);
        }

        [MonoPInvokeCallback(typeof(internal_AnchorReceived))]
        static void _anchor_received(IntPtr anchorPtr)
        {
            var anchorData = UnityARUserAnchorDataFromARAnchorPtr(anchorPtr);
            AnchorReceivedInternal?.Invoke(anchorData);
        }

        [MonoPInvokeCallback(typeof(internal_StateChanged))]
        static void _state_changed(UnityMCPeerID peerId, UnityMCSessionState sessionState)
        {
            StateChangedInternal?.Invoke(peerId, sessionState);
        }

        void WorldMapReceived(ARWorldMap worldMap)
        {
            WorldMapReceivedEvent?.Invoke(worldMap);
        }

        void AnchorReceived(UnityARUserAnchorData anchorData)
        {
            AnchorReceivedEvent?.Invoke(anchorData);
        }

        void StateChanged(UnityMCPeerID peerId, UnityMCSessionState sessionState)
        {
            StateChangedEvent?.Invoke(peerId, sessionState);
        }
    }
}
