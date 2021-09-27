﻿using System;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Response
{
    public class GameDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }

        public GameDetailDTO()
        {
        }

        public GameDetailDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
            Synopsis = game.Synopsis;
            Gender = game.Gender;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Synopsis = attributes[2];
            Gender = attributes[3];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Title}~~{Synopsis}~~{Gender}");
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}\n\tSynopsis: {Synopsis}\n\tGender = {Gender}";
        }
    }
}