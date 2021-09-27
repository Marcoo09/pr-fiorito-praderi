using System.Text;
using Server.Domain;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Protocol.SerializationInterfaces;

namespace DTOs.Request
{
    public class CreateGameDTO : ISerializable, IDeserializable
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Synopsis { get; set; }

        public string CoverName { get; set; }
        public byte[] Data { get; set; }
        public long FileSize { get; set; }

        public Game ToEntity()
        {
            return new Game()
            {
                Title = Title,
                Synopsis = Synopsis,
                Gender = Gender,
                CoverName = CoverName,
                Path = Directory.GetCurrentDirectory() + "\\" + CoverName,
                FileSize = FileSize
            };
        }

        public byte[] Serialize()
        {
            List<byte> serializedGame = new List<byte>();
            serializedGame.AddRange(BitConverter.GetBytes(FileSize));
            serializedGame.AddRange(Data);
            serializedGame.AddRange(Encoding.UTF8.GetBytes($"{Title}~~{Gender}~~{Synopsis}~~{CoverName}"));

            return serializedGame.ToArray();
        }

        public void Deserialize(byte[] serializedEntity)
        {
    
            int offset = 0;
            FileSize = BitConverter.ToInt64(serializedEntity.Take(8).ToArray());
            offset += 8;
            Data = serializedEntity.Skip(offset).Take((int)FileSize).ToArray();
            offset += (int)FileSize;

            Title = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());
            Gender = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());
            Synopsis = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());

            string[] attributes = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray()).Split("~~");

            Title = attributes[0];
            Gender = attributes[1];
            Synopsis = attributes[2];
            CoverName = attributes[3];
        }

        public bool IsValidPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                return false;

            return File.Exists(path);
        }

        public void ReadFile(string path)
        {
            Data = File.ReadAllBytes(path);
            CoverName = new FileInfo(path).Name;
            FileSize = new FileInfo(path).Length;
        }

        public void WriteFile()
        {
            File.WriteAllBytes(CoverName, Data);
        }
    }
}
