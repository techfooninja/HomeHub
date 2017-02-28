﻿namespace HomeHub.Client
{
    using Newtonsoft.Json;
    using Shared;
    using System;

    public class ClientJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(IThermostat) ||
                objectType == typeof(IDevice))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IThermostat))
                return serializer.Deserialize(reader, typeof(ThermostatProxy));
            else if (objectType == typeof(IDevice))
                return serializer.Deserialize(reader, typeof(DeviceProxy));

            throw new NotSupportedException(string.Format("Type {0} unexpected.", objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
