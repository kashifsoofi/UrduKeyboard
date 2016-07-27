using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeyboardLib
{
    public class NativeTypeConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(nfloat) ||
                objectType == typeof(nint) ||
                objectType == typeof(nuint))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (objectType == typeof(nfloat) ||
                objectType == typeof(nfloat?))
            {
                float floatValue = (float)token;
                return (nfloat)floatValue;
            }
            else if (objectType == typeof(nint))
            {
                int intValue = (int)token;
                return (nint)intValue;
            }
            else {
                uint uintValue = (uint)token;
                return (nuint)uintValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

