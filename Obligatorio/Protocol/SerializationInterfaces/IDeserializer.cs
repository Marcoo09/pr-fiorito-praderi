using System;
using System.Collections.Generic;

namespace Protocol.Serialization
{
    public interface IDeserializer
    {
        List<IDeserializable> DeserializeArrayOfEntities(byte[] serializedEntities, Type entityType);
        IDeserializable DeserializeEntity(byte[] serializedEntity, Type entityType);
    }
}
