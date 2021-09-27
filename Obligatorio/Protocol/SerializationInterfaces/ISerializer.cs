using System;
using System.Collections.Generic;

namespace Protocol.SerializationInterfaces
{
    public interface ISerializer
    {
        byte[] SerializeEntity(ISerializable entity);
        byte[] SerializeEntityList(List<ISerializable> entityList);
    }
}
