using System;
using System.Collections.Generic;
using System.Linq;

namespace Protocol.Serialization
{
    public class Serializer : ISerializer
    {
        public byte[] SerializeEntity(ISerializable entity)
        {
            return entity.Serialize();
        }

        public byte[] SerializeEntityList(List<ISerializable> entityList)
        {
            List<byte[]> serializedEntities = entityList.Select(entity => entity.Serialize()).ToList();
            List<byte> joinedEntities = new List<byte>();

            byte separator = 124; //ASCII'|'

            for (int i = 0; i < serializedEntities.Count; i++)
            {
                byte[] serializedEntity = serializedEntities.ElementAt(i);
                joinedEntities.AddRange(serializedEntity);

                if (i + 1 < serializedEntities.Count)
                    joinedEntities.Add(separator);
            }

            return joinedEntities.ToArray();
        }
    }
}