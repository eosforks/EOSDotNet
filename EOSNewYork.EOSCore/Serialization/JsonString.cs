﻿using System;
using System.IO;
using System.Text;
using Cryptography.ECDSA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EOSNewYork.EOSCore.Serialization
{
    public class JsonString : BaseCustomType
    {
        public string value { get; set; }

        public JsonString() { }

        public JsonString(string value)
        {
            this.value = value;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JTokenType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            value = serializer.Deserialize<string>(reader);
            var @object = (JsonString)Activator.CreateInstance(objectType, value);
            return @object;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //serialize as actual JSON and not string data
            var token = JToken.Parse(value.ToString());
            writer.WriteToken(token.CreateReader());

        }

        public override void WriteToStream(Stream stream)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var length = new UnsignedInt((uint)bytes.Length);
            length.WriteToStream(stream);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
