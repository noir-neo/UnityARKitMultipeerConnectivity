/*
See LICENSE folder for this sample’s licensing information.

Abstract:
A simple abstraction of the MultipeerConnectivity API as used in this app.
*/

import Foundation
import MultipeerConnectivity
import ARKit

@objcMembers
class UnityMCSession: NSObject {
    static let serviceType = "ar-multi-sample"
    
    private let myPeerID = MCPeerID(displayName: UIDevice.current.name)
    var session: MCSession!
    private var serviceAdvertiser: MCNearbyServiceAdvertiser!
    private var serviceBrowser: MCNearbyServiceBrowser!
    
    private var dataReceived: UNITY_MC_ARRAY_CALLBACK!
    private var stateChanged: UNITY_MC_STATE_CALLBACK!
    
    override init() {
        super.init()
        
        session = MCSession(peer: myPeerID, securityIdentity: nil, encryptionPreference: .required)
        session.delegate = self
        
        serviceAdvertiser = MCNearbyServiceAdvertiser(peer: myPeerID, discoveryInfo: nil, serviceType: UnityMCSession.serviceType)
        serviceAdvertiser.delegate = self
        serviceAdvertiser.startAdvertisingPeer()
        
        serviceBrowser = MCNearbyServiceBrowser(peer: myPeerID, serviceType: UnityMCSession.serviceType)
        serviceBrowser.delegate = self
        serviceBrowser.startBrowsingForPeers()
    }
    
    func sendToAllPeers(_ data: Data) {
        do {
            try session.send(data, toPeers: session.connectedPeers, with: .reliable)
        } catch {
            print("error sending data to peers: \(error.localizedDescription)")
        }
    }
    
    func setCallbacks(_ dataReceived: @escaping UNITY_MC_ARRAY_CALLBACK, stateChanged: @escaping UNITY_MC_STATE_CALLBACK) {
        self.dataReceived = dataReceived
        self.stateChanged = stateChanged
    }
    
    func receivedDataHandler(_ data: Data, from peer: MCPeerID) {
        data.withUnsafeBytes { (ptr: UnsafePointer<UInt8>) in
            let rawPtr = UnsafeMutableRawPointer(mutating: ptr)
            DispatchQueue.main.async {
                self.dataReceived(rawPtr, CInt(data.count))
            }
        }
    }
    
    func stateChangedHandler(_ peerID: MCPeerID, didChange state: MCSessionState)
    {
        DispatchQueue.main.async {
            self.stateChanged(peerID.toUnity(), state.toUnity())
        }
    }
    
    var connectedPeers: [MCPeerID] {
        return session.connectedPeers
    }
}

extension UnityMCSession: MCSessionDelegate {
    
    public func session(_ session: MCSession, peer peerID: MCPeerID, didChange state: MCSessionState) {
        stateChangedHandler(peerID, didChange: state)
    }
    
    public func session(_ session: MCSession, didReceive data: Data, fromPeer peerID: MCPeerID) {
        receivedDataHandler(data, from: peerID)
    }
    
    public func session(_ session: MCSession, didReceive stream: InputStream, withName streamName: String, fromPeer peerID: MCPeerID) {
        fatalError("This service does not send/receive streams.")
    }
    
    public func session(_ session: MCSession, didStartReceivingResourceWithName resourceName: String, fromPeer peerID: MCPeerID, with progress: Progress) {
        fatalError("This service does not send/receive resources.")
    }
    
    public func session(_ session: MCSession, didFinishReceivingResourceWithName resourceName: String, fromPeer peerID: MCPeerID, at localURL: URL?, withError error: Error?) {
        fatalError("This service does not send/receive resources.")
    }
    
}

extension UnityMCSession: MCNearbyServiceBrowserDelegate {
    
    /// - Tag: FoundPeer
    public func browser(_ browser: MCNearbyServiceBrowser, foundPeer peerID: MCPeerID, withDiscoveryInfo info: [String: String]?) {
        // Invite the new peer to the session.
        browser.invitePeer(peerID, to: session, withContext: nil, timeout: 10)
    }

    public func browser(_ browser: MCNearbyServiceBrowser, lostPeer peerID: MCPeerID) {
        // This app doesn't do anything with non-invited peers, so there's nothing to do here.
    }
    
}

extension UnityMCSession: MCNearbyServiceAdvertiserDelegate {
    
    /// - Tag: AcceptInvite
    public func advertiser(_ advertiser: MCNearbyServiceAdvertiser, didReceiveInvitationFromPeer peerID: MCPeerID, withContext context: Data?, invitationHandler: @escaping (Bool, MCSession?) -> Void) {
        // Call handler to accept invitation and join the session.
        invitationHandler(true, self.session)
    }
    
}
