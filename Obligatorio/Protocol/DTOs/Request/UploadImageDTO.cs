using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Request
{
    public class UploadImageDTO : ISerializable, IDeserializable
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int GameId { get; set; }
        public DateTime UploadedAt { get; set; }
        public long FileSize { get; set; }

        public bool IsValidPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            return File.Exists(path);
        }

        public void ReadFile(string path)
        {
            Data = File.ReadAllBytes(path);
            Name = new FileInfo(path).Name;
            FileSize = new FileInfo(path).Length;
        }

        public void WriteFile()
        {
            File.WriteAllBytes(Name, Data);
        }

        public Document ToEntity()
        {
            return new Document()
            {
                Name = Name,
                UploadedAt = UploadedAt,
                Path = Directory.GetCurrentDirectory() + "\\" + Name,
                FileSize = FileSize
            };
        }

        public byte[] Serialize()
        {
            List<byte> serializedDocument = new List<byte>();
            serializedDocument.AddRange(BitConverter.GetBytes(FileSize));
            serializedDocument.AddRange(Data);
            serializedDocument.AddRange(BitConverter.GetBytes(GameId));
            serializedDocument.AddRange(BitConverter.GetBytes(UploadedAt.Ticks));
            serializedDocument.AddRange(Encoding.UTF8.GetBytes(Name));

            return serializedDocument.ToArray();
        }

        public void Deserialize(byte[] entity)
        {
            int offset = 0;
            FileSize = BitConverter.ToInt64(entity.Take(8).ToArray());
            offset += 8;
            Data = entity.Skip(offset).Take((int)FileSize).ToArray();
            offset += (int)FileSize;
            GameId = BitConverter.ToInt32(entity.Skip(offset).Take(4).ToArray());
            offset += 4;
            UploadedAt = new DateTime(BitConverter.ToInt64(entity.Skip(offset).Take(8).ToArray()));
            offset += 8;
            Name = Encoding.UTF8.GetString(entity.Skip(offset).ToArray());
        }
    }
}