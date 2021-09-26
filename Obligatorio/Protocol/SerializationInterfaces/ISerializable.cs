using System;
namespace Protocol.Serialization
{
    public interface ISerializable
    {
        byte[] Serialize();
    }
}
