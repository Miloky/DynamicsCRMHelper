using System;
using Newtonsoft.Json;

namespace DynamicsHelper.Extensions.Newtonsoft
{
    public class UnixTimeConverter : JsonConverter<DateTime>
    {
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            string timeString = reader.Value.ToString();
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(Int32.Parse(timeString)).ToLocalTime();
        }
    }
}