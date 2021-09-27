using System.Text;
using Server.Domain;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTOs.Request
{
    public class CreateGameDTO
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
            List<byte> serializedDocument = new List<byte>();
            serializedDocument.AddRange(BitConverter.GetBytes(FileSize));
            serializedDocument.AddRange(Data);
            serializedDocument.AddRange(Encoding.UTF8.GetBytes(Title));
            serializedDocument.AddRange(Encoding.UTF8.GetBytes(Gender));
            serializedDocument.AddRange(Encoding.UTF8.GetBytes(Synopsis));

            return serializedDocument.ToArray();
        }

        public void Deserialize(byte[] serializedEntity)
        {
            string[] attributes = Encoding.UTF8.GetString(serializedEntity).Split("~~");

            Title = attributes[0];
            Synopsis = attributes[1];
            Gender = attributes[2];

            int offset = 0;
            FileSize = BitConverter.ToInt64(serializedEntity.Take(8).ToArray());
            offset += 8;
            Data = serializedEntity.Skip(offset).Take((int)FileSize).ToArray();
            offset += (int)FileSize;
            Title = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());
            offset += 8;
            Gender = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());
            offset += 8;
            Synopsis = Encoding.UTF8.GetString(serializedEntity.Skip(offset).ToArray());
            offset += 8;
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
