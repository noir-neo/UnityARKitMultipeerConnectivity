#import <Foundation/Foundation.h>
#import <ARKit/ARKit.h>
#import <MultipeerConnectivity/MultipeerConnectivity.h>
#import "unityswift-Swift.h"

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

    void _setCallbacks(const void* nativeSession, UNITY_MC_WORLD_MAP_CALLBACK worldMapReceived) {
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        [session setCallbacks:(UNITY_MC_WORLD_MAP_CALLBACK) worldMapReceived];
    }
}
