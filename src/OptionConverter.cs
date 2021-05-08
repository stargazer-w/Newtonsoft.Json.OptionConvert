using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Optional;

namespace Stargazer.Extensions.Newtonsoft.Json.OptionConvert
{
    public class OptionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Option<>);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            Type valueTypeToConvert = objectType.GetGenericArguments()[0];
            var value = serializer.Deserialize(reader, valueTypeToConvert);
            return Activator.CreateInstance(objectType,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { value, true },
                null
            );
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
