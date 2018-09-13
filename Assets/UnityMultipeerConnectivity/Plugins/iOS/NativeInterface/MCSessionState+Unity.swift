import MultipeerConnectivity

extension MCSessionState {
    func toUnity() -> UnityMCSessionState {
        switch self {
        case .notConnected:
            return UnityMCSessionState.notConnected
        case .connecting:
            return UnityMCSessionState.connecting
        case .connected:
            return UnityMCSessionState.connected
        }
    }
}

