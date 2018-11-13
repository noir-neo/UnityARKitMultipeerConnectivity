using MessagePack;
using UnityEngine.XR.iOS;

[MessagePackObject]
public struct PackableARWorldMap
{
    [Key(0)]
    public readonly byte[] ARWorldMapData;

    [SerializationConstructor]
    public PackableARWorldMap(byte[] arWorldMapData)
    {
        ARWorldMapData = arWorldMapData;
    }

    public static explicit operator ARWorldMap(PackableARWorldMap arWorldMap)
    {
        return ARWorldMap.SerializeFromByteArray(arWorldMap.ARWorldMapData);
    }

    public static explicit operator PackableARWorldMap(ARWorldMap arWorldMap)
    {
        return new PackableARWorldMap(arWorldMap.SerializeToByteArray());
    }
}
