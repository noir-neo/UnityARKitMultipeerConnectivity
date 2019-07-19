using MessagePack.Resolvers;
using UnityEngine;

public class MessagePackResolverRegister : MonoBehaviour
{
    void Start()
    {
        CompositeResolver.RegisterAndSetAsDefault(
            GeneratedResolver.Instance,
            MessagePack.Unity.UnityResolver.Instance,
            BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,
            PrimitiveObjectResolver.Instance
            );
    }
}
