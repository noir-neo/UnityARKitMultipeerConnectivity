using System.Runtime.InteropServices;
using MessagePack;
using UnityEngine;
using UnityEngine.XR.iOS;

[MessagePackObject]
public struct PackableARUserAnchor : IMessagePackUnion
{
    [Key(0)]
    public readonly string Identifier;

    [Key(1)]
    public readonly Matrix4x4 Transform;

    [SerializationConstructor]
    public PackableARUserAnchor(string identifier, Matrix4x4 transform)
    {
        Identifier = identifier;
        Transform = transform;
    }

    public static explicit operator UnityARUserAnchorData(PackableARUserAnchor anchor)
    {
        // cannot construct with identifier
        UnityARUserAnchorData ad = new UnityARUserAnchorData
        {
            transform =
            {
                column0 = anchor.Transform.GetColumn(0),
                column1 = anchor.Transform.GetColumn(1),
                column2 = anchor.Transform.GetColumn(2),
                column3 = anchor.Transform.GetColumn(3)
            }
        };
        return ad;
    }

    public static explicit operator PackableARUserAnchor(UnityARUserAnchorData anchor)
    {
        return new PackableARUserAnchor(
            Marshal.PtrToStringAuto(anchor.ptrIdentifier),
            new Matrix4x4(
                anchor.transform.column0,
                anchor.transform.column1,
                anchor.transform.column2,
                anchor.transform.column3)
            );
    }
}
