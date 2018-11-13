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

typedef void (*UNITY_MC_ARRAY_CALLBACK)(void*, int);
typedef void (*UNITY_MC_STATE_CALLBACK)(UnityMCPeerID, UnityMCSessionState);
