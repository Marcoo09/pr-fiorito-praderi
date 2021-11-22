using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Logger.Domain
{
    public class Log
    {
        public DateTime CreatedAt { get; set; }
        public object Entity { get; set; }
        public Tag Tag { get; set; }
        public Type EntityType { get; set; }

        public bool IsEntityAList()
        {
            return EntityType.IsGenericType && EntityType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public override string ToString()
        {
            string result;

            if (IsEntityAList())
            {
                IList entityList = Entity as IList;
                result = $"[{CreatedAt}] ({Tag}) - \n";

                foreach (var entity in entityList)
                {
                    result += $"{entity} \n";
                }
            }
            else
            {
                result = $"[{CreatedAt}] ({Tag}) - {Entity.ToString()}";
            }

            return result;
        }
    }
}