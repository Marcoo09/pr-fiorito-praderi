using System;
using Protocol.SerializationInterfaces;
using System.Text;

namespace DTOs.Request
{
    public class SearchMetricDTO : ISerializable, IDeserializable
    {
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("#");

            Rating = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Gender = attributes[2];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Rating}#{Title}#{Gender}");
        }
    }
}
