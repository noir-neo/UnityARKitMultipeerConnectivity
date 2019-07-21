using System;
using MessagePack;
using UnityEngine.XR.ARKit;

[MessagePackObject]
public struct PackableARCollaborationData : IMessagePackUnion
{
    [Key(0)]
    public readonly byte[] ARCollaborationData;

    [SerializationConstructor]
    public PackableARCollaborationData(byte[] arCollaborationData)
    {
        ARCollaborationData = arCollaborationData;
    }

    public static explicit operator ARCollaborationData(PackableARCollaborationData packedCollaborationData)
    {
        var collaborationData = new ARCollaborationData(packedCollaborationData.ARCollaborationData);
        if (!collaborationData.valid)
        {
            throw new Exception("Data is not a valid ARCollaborationData");
        }
        return collaborationData;
    }

    public static explicit operator PackableARCollaborationData(ARCollaborationData arCollaborationData)
    {
        return new PackableARCollaborationData(arCollaborationData.ToSerialized().bytes.ToArray());
    }
}