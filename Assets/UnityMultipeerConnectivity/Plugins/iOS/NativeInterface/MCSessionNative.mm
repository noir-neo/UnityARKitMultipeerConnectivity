#import <Foundation/Foundation.h>
#import <ARKit/ARKit.h>
#import <MultipeerConnectivity/MultipeerConnectivity.h>
#import "unityswift-Swift.h"
#import "ARKitDefines.h"

extern "C" {
    void* _createNativeMCSession() {
        UnityMCSession* session = [[UnityMCSession alloc] init];
        return (__bridge_retained void*)session;
    }

    void _sendARWorldMapToAllPeers(const void* nativeSession, const void* worldMapPtr) {
        if (worldMapPtr == nullptr)
            return;
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        ARWorldMap* worldMap = (__bridge ARWorldMap*)worldMapPtr;
        [session sendToAllPeers:(ARWorldMap*) worldMap];
    }
    
    void _sendARAnchorToAllPeers(const void* nativeSession, UnityARUserAnchorData anchorData) {
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        matrix_float4x4 matrix;
        UnityARMatrix4x4ToARKitMatrix(anchorData.transform, &matrix);
        ARAnchor *anchor = [[ARAnchor alloc] initWithTransform:matrix];
        [session sendToAllPeers:(ARAnchor*) anchor];
    }

    void _setCallbacks(const void* nativeSession, UNITY_MC_WORLD_MAP_CALLBACK worldMapReceived, UNITY_MC_ANCHOR_CALLBACK anchorReceived) {
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        [session setCallbacks:(UNITY_MC_WORLD_MAP_CALLBACK) worldMapReceived anchorReceived: (UNITY_MC_ANCHOR_CALLBACK) anchorReceived];
    }
    
    UnityARUserAnchorData _unityARUserAnchorDataFromARAnchorPtr(const void* anchorPtr) {
        ARAnchor* anchor = (__bridge ARAnchor*)anchorPtr;
        UnityARUserAnchorData anchorData;
        UnityARUserAnchorDataFromARAnchorPtr(anchorData, anchor);
        return anchorData;
    }
}
