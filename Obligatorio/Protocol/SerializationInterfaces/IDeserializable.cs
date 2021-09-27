using System;
namespace Protocol.SerializationInterfaces
{
    public interface IDeserializable
    {
        void Deserialize(byte[] entity);
    }
}
