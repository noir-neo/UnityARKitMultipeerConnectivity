#import "MCPeerID+Unity.h"
#import "MCDefines.h"

@implementation MCPeerID (Unity)
- (UnityMCPeerID) toUnity
{
    UnityMCPeerID unityPeerID;
    unityPeerID.displayName = (void*)[self.displayName UTF8String];
    return unityPeerID;
}
@end

