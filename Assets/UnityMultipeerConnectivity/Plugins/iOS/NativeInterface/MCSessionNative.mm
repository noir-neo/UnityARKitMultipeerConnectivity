#import <Foundation/Foundation.h>
#import <ARKit/ARKit.h>
#import "unityswift-Swift.h"

extern "C" {
    void* _createNativeMCSession() {
        UnityMCSession* session = [[UnityMCSession alloc] init];
        return (__bridge_retained void*)session;
    }

    void _sendToAllPeers(const void* nativeSession, const char** arr, const int length) {
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        NSData* data = [NSData dataWithBytes:(const void *)arr length:(sizeof(unsigned char) * length)];
        [session sendToAllPeers:(NSData*) data];
    }

    void _setCallbacks(const void* nativeSession, UNITY_MC_ARRAY_CALLBACK dataReceived, UNITY_MC_STATE_CALLBACK stateChanged) {
        UnityMCSession* session = (__bridge UnityMCSession*)nativeSession;
        [session setCallbacks:(UNITY_MC_ARRAY_CALLBACK) dataReceived
            stateChanged:(UNITY_MC_STATE_CALLBACK) stateChanged
            ];
    }
}
