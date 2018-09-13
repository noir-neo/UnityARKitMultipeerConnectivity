typedef NS_ENUM(NSInteger, UnityMCSessionState)
{
    UnityMCSessionStateNotConnected = 0,
    UnityMCSessionStateConnecting = 1 << 0,
    UnityMCSessionStateConnected = 1 << 1
};

typedef struct
{
    void* displayName;
} UnityMCPeerID;

typedef void (*UNITY_MC_WORLD_MAP_CALLBACK)(void*);
typedef void (*UNITY_MC_ANCHOR_CALLBACK)(void*);
typedef void (*UNITY_MC_STATE_CALLBACK)(UnityMCPeerID, UnityMCSessionState);
