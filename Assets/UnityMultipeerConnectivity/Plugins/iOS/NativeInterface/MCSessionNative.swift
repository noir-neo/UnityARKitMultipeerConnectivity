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
    
    private var worldMapReceived: UNITY_MC_WORLD_MAP_CALLBACK!
    private var anchorReceived: UNITY_MC_ANCHOR_CALLBACK!

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

    func sendToAllPeers(_ object: Any) {
        
        guard let data = try? NSKeyedArchiver.archivedData(withRootObject: object, requiringSecureCoding: true)
            else { fatalError("can't encode object") }
        self.sendToAllPeers(data)
    }

    private func sendToAllPeers(_ data: Data) {
        do {
            try session.send(data, toPeers: session.connectedPeers, with: .reliable)
        } catch {
            print("error sending data to peers: \(error.localizedDescription)")
        }
    }
    
    func setCallbacks(_ worldMapReceived: @escaping UNITY_MC_WORLD_MAP_CALLBACK, anchorReceived: @escaping UNITY_MC_ANCHOR_CALLBACK) {
        self.worldMapReceived = worldMapReceived
        self.anchorReceived = anchorReceived
    }

    func receivedDataHandler(_ data: Data, from peer: MCPeerID) {
        
        if let unarchived = try? NSKeyedUnarchiver.unarchivedObject(of: ARWorldMap.classForKeyedUnarchiver(), from: data),
            let worldMap = unarchived as? ARWorldMap {
            let unmanaged = Unmanaged.passRetained(worldMap)
            let ptr = unmanaged.toOpaque()
            worldMapReceived(ptr)
            unmanaged.release()
        }
        else if let unarchived = try? NSKeyedUnarchiver.unarchivedObject(of: ARAnchor.classForKeyedUnarchiver(), from: data),
            let anchor = unarchived as? ARAnchor {
            let unmanaged = Unmanaged.passRetained(anchor)
            let ptr = unmanaged.toOpaque()
            anchorReceived(ptr)
            unmanaged.release()
        }
        else {
            print("unknown data recieved from \(peer)")
        }
    }
    
    var connectedPeers: [MCPeerID] {
        return session.connectedPeers
    }
}

extension UnityMCSession: MCSessionDelegate {
    
    public func session(_ session: MCSession, peer peerID: MCPeerID, didChange state: MCSessionState) {
        // not used
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
