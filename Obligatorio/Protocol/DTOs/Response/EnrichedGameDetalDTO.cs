using System;
using System.Linq;
using System.IO;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DTOs.Response
{
    public class EnrichedGameDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }
        public string Path { get; set; }
        public double RatingAverage { get; set; }

        public string CoverName { get; set; }
        public byte[] Data { get; set; }
        public long FileSize { get; set; }

        public EnrichedGameDetailDTO()
        {
        }

        public EnrichedGameDetailDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
            Synopsis = game.Synopsis;
            Gender = game.Gender;
            CoverName = game.CoverName;
            RatingAverage = game.Reviews.Count > 0 ? game.Reviews.Select(g => g.Rating).ToList().Average() : 0.0;
        }

        public void Deserialize(byte[] entity)
        {
            //int offset = 0;
            //FileSize = BitConverter.ToInt64(entity.Take(8).ToArray());
            //offset += 8;
            //Data = entity.Skip(offset).Take((int)FileSize).ToArray();
            //offset += (int)FileSize;

            //string[] attributes = Encoding.UTF8.GetString(entity.Skip(offset).ToArray()).Split("#");
            string[] attributes = Encoding.UTF8.GetString(entity.ToArray()).Split("#");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Synopsis = attributes[2];
            Gender = attributes[3];
            RatingAverage = Double.Parse(attributes[4]);
            CoverName = attributes[5];

        }

        public byte[] Serialize()
        {
            List<byte> serializedGame = new List<byte>();
            //serializedGame.AddRange(BitConverter.GetBytes(FileSize));
            //serializedGame.AddRange(Data);
            serializedGame.AddRange(Encoding.UTF8.GetBytes($"{Id}#{Title}#{Synopsis}#{Gender}#{RatingAverage}#{CoverName}"));

            return serializedGame.ToArray();
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
            Path = Directory.GetCurrentDirectory() + (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "/" : "\\") + CoverName;
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tTitle: {Title}\n\tSynopsis: {Synopsis}\n\tGender: {Gender}\n\tDownload Cover Path: {Path}\n\tAverage Rating: {RatingAverage}";
        }
    }
}
