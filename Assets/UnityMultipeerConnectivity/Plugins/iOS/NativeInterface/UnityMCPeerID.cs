using System;
using System.Runtime.InteropServices;

namespace UnityMultipeerConnectivity
{
    public struct UnityMCPeerID
    {
        readonly IntPtr displayName;
        public string DisplayName => Marshal.PtrToStringAuto(displayName);
    }
}
