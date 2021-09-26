using System;
using System.Collections.Generic;
using System.Linq;

namespace Protocol.Serialization
{
    public class Deserializer : IDeserializer
    {
        public List<IDeserializable> DeserializeArrayOfEntities(byte[] serializedEntities, Type entityType)
        {
            string separator = "|";
            List<List<byte>> separatedEntities = new List<List<byte>>();
            int currentEntity = 1;

            foreach (byte singleByte in serializedEntities)
            {
                if (System.Text.Encoding.UTF8.GetString(new byte[] { singleByte }) == separator)
                    currentEntity++;
                else
                {
                    if (separatedEntities.Count < currentEntity)
                    {
                        List<byte> newEntity = new List<byte>();
                        newEntity.Add(singleByte);
                        separatedEntities.Add(newEntity);
                    }
                    else
                    {
                        List<byte> entityInProgress = separatedEntities.ElementAt(currentEntity - 1);
                        entityInProgress.Add(singleByte);
                    }
                }

            }
            List<IDeserializable> deserializedEntities = new List<IDeserializable>();

            foreach (List<byte> entity in separatedEntities)
            {
                IDeserializable deserializedEntity = (IDeserializable)Activator.CreateInstance(entityType);
                deserializedEntity.Deserialize(entity.ToArray());
                deserializedEntities.Add(deserializedEntity);
            }

            return deserializedEntities;
        }

        public IDeserializable DeserializeEntity(byte[] serializedEntity, Type entityType)
        {
            IDeserializable deserializedEntity = (IDeserializable)Activator.CreateInstance(entityType);
            deserializedEntity.Deserialize(serializedEntity);
            return deserializedEntity;
        }
    }
}