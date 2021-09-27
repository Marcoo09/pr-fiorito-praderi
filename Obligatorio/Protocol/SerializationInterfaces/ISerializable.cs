using System;
namespace Protocol.SerializationInterfaces
{
    public interface ISerializable
    {
        byte[] Serialize();
    }
}
