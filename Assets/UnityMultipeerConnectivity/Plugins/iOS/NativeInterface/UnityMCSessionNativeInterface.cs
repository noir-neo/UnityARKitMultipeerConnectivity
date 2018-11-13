using System;
using System.Runtime.InteropServices;
using AOT;

namespace UnityMultipeerConnectivity
{
    public sealed class UnityMCSessionNativeInterface : IDisposable
    {
        static UnityMCSessionNativeInterface mcSessionNativeInterface;
        public static UnityMCSessionNativeInterface GetMcSessionNativeInterface()
        {
            return mcSessionNativeInterface ?? (mcSessionNativeInterface = new UnityMCSessionNativeInterface());
        }

        public event Action<byte[]> DataReceivedEvent;
        static event Action<byte[]> DataReceivedInternal;
        delegate void internal_DataReceived(IntPtr arrPtr, int length);

        public event Action<UnityMCPeerID, UnityMCSessionState> StateChangedEvent;
        static event Action<UnityMCPeerID, UnityMCSessionState> StateChangedInternal;
        delegate void internal_StateChanged(UnityMCPeerID peerId, UnityMCSessionState sessionState);

#if !UNITY_EDITOR && UNITY_IOS
        readonly IntPtr nativeMCSession;
#endif

        UnityMCSessionNativeInterface()
        {
            DataReceivedInternal += DataReceived;
            StateChangedInternal += StateChanged;
#if !UNITY_EDITOR && UNITY_IOS
            nativeMCSession = _createNativeMCSession();
            _setCallbacks(nativeMCSession, _data_received, _state_changed);
#endif
        }

        public void Dispose()
        {
            DataReceivedInternal -= DataReceived;
            StateChangedInternal -= StateChanged;
        }

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        static extern IntPtr _createNativeMCSession();

        [DllImport("__Internal")]
        static extern void _setCallbacks(IntPtr nativeSession, internal_DataReceived dataReceivedCallback, internal_StateChanged stateChangedCallback);

        [DllImport("__Internal")]
        static extern void _sendToAllPeers(IntPtr nativeSession, byte[] array, int length);
#endif

        public void SendToAllPeers(byte[] data)
        {
#if !UNITY_EDITOR && UNITY_IOS
            _sendToAllPeers(nativeMCSession, data, data.Length);
#endif
        }

        [MonoPInvokeCallback(typeof(internal_DataReceived))]
        static void _data_received(IntPtr arrPtr, int length)
        {
            var array = new byte[length];
            Marshal.Copy(arrPtr, array, 0, length);
            DataReceivedInternal?.Invoke(array);
        }

        [MonoPInvokeCallback(typeof(internal_StateChanged))]
        static void _state_changed(UnityMCPeerID peerId, UnityMCSessionState sessionState)
        {
            StateChangedInternal?.Invoke(peerId, sessionState);
        }

        void DataReceived(byte[] data)
        {
            DataReceivedEvent?.Invoke(data);
        }

        void StateChanged(UnityMCPeerID peerId, UnityMCSessionState sessionState)
        {
            StateChangedEvent?.Invoke(peerId, sessionState);
        }
    }
}
