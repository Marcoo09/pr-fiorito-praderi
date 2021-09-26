using System;
namespace Protocol.Serialization
{
    public interface IDeserializable
    {
        void Deserialize(byte[] entity);
    }
}
