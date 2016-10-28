﻿using Newtonsoft.Json;
using NJsonApi.Serialization.Converters;
using System;

namespace NJsonApi.Serialization.Representations
{
    [JsonConverter(typeof(SerializationAwareConverter))]
    public class SimpleLink : ILink, ISerializationAware
    {
        public SimpleLink()
        {
        }

        public SimpleLink(string href)
        {
            this.Href = href;
        }

        public string Href { get; set; }

        public void Serialize(JsonWriter writer) => writer.WriteValue(Href);

        public override string ToString() => Href;
    }
}