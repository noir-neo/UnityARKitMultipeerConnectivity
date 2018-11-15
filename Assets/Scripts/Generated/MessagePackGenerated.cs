#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(3)
            {
                {typeof(global::IMessagePackUnion), 0 },
                {typeof(global::PackableARUserAnchor), 1 },
                {typeof(global::PackableARWorldMap), 2 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.IMessagePackUnionFormatter();
                case 1: return new MessagePack.Formatters.PackableARUserAnchorFormatter();
                case 2: return new MessagePack.Formatters.PackableARWorldMapFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters
{
    using System;
    using System.Collections.Generic;
    using MessagePack;

    public sealed class IMessagePackUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::IMessagePackUnion>
    {
        readonly Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
        readonly Dictionary<int, int> keyToJumpMap;

        public IMessagePackUnionFormatter()
        {
            this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(2, global::MessagePack.Internal.RuntimeTypeHandleEqualityComparer.Default)
            {
                { typeof(global::PackableARWorldMap).TypeHandle, new KeyValuePair<int, int>(0, 0) },
                { typeof(global::PackableARUserAnchor).TypeHandle, new KeyValuePair<int, int>(1, 1) },
            };
            this.keyToJumpMap = new Dictionary<int, int>(2)
            {
                { 0, 0 },
                { 1, 1 },
            };
        }

        public int Serialize(ref byte[] bytes, int offset, global::IMessagePackUnion value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            KeyValuePair<int, int> keyValuePair;
            if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
                offset += MessagePackBinary.WriteInt32(ref bytes, offset, keyValuePair.Key);
                switch (keyValuePair.Value)
                {
                    case 0:
                        offset += formatterResolver.GetFormatterWithVerify<global::PackableARWorldMap>().Serialize(ref bytes, offset, (global::PackableARWorldMap)value, formatterResolver);
                        break;
                    case 1:
                        offset += formatterResolver.GetFormatterWithVerify<global::PackableARUserAnchor>().Serialize(ref bytes, offset, (global::PackableARUserAnchor)value, formatterResolver);
                        break;
                    default:
                        break;
                }

                return offset - startOffset;
            }

            return MessagePackBinary.WriteNil(ref bytes, offset);
        }
        
        public global::IMessagePackUnion Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            
            if (MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize) != 2)
            {
                throw new InvalidOperationException("Invalid Union data was detected. Type:global::IMessagePackUnion");
            }
            offset += readSize;

            var key = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            offset += readSize;

            if (!this.keyToJumpMap.TryGetValue(key, out key))
            {
                key = -1;
            }

            global::IMessagePackUnion result = null;
            switch (key)
            {
                case 0:
                    result = (global::IMessagePackUnion)formatterResolver.GetFormatterWithVerify<global::PackableARWorldMap>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                case 1:
                    result = (global::IMessagePackUnion)formatterResolver.GetFormatterWithVerify<global::PackableARUserAnchor>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                default:
                    offset += MessagePackBinary.ReadNextBlock(bytes, offset);
                    break;
            }
            
            readSize = offset - startOffset;
            
            return result;
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters
{
    using System;
    using MessagePack;


    public sealed class PackableARUserAnchorFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::PackableARUserAnchor>
    {

        public int Serialize(ref byte[] bytes, int offset, global::PackableARUserAnchor value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Identifier, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::UnityEngine.Matrix4x4>().Serialize(ref bytes, offset, value.Transform, formatterResolver);
            return offset - startOffset;
        }

        public global::PackableARUserAnchor Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Identifier__ = default(string);
            var __Transform__ = default(global::UnityEngine.Matrix4x4);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Identifier__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Transform__ = formatterResolver.GetFormatterWithVerify<global::UnityEngine.Matrix4x4>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::PackableARUserAnchor(__Identifier__, __Transform__);
            return ____result;
        }
    }


    public sealed class PackableARWorldMapFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::PackableARWorldMap>
    {

        public int Serialize(ref byte[] bytes, int offset, global::PackableARWorldMap value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += formatterResolver.GetFormatterWithVerify<byte[]>().Serialize(ref bytes, offset, value.ARWorldMapData, formatterResolver);
            return offset - startOffset;
        }

        public global::PackableARWorldMap Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __ARWorldMapData__ = default(byte[]);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __ARWorldMapData__ = formatterResolver.GetFormatterWithVerify<byte[]>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::PackableARWorldMap(__ARWorldMapData__);
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
