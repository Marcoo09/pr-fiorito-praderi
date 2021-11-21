using Protocol.SerializationInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Request
{
    public class IndexFilesDTO : ISerializable, IDeserializable
    {
        public int ReviewId { get; set; }
        public int OrderBy { get; set; }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{ReviewId}~~{OrderBy}");
        }

        public void Deserialize(byte[] serializedEntity)
        {
            string[] attributes = Encoding.UTF8.GetString(serializedEntity).Split("~~");
            ReviewId = Int32.Parse(attributes[0]);
            OrderBy = Int32.Parse(attributes[1]);
        }
    }
}
