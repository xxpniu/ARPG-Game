using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace org.vxwo.csharp.json
{
	class JsonSerializer
    {
        private StringBuilder output = new StringBuilder();
		private bool serializeZeros = true;
        private bool serializeNulls = true;
        private const int MAX_DEPTH = 10;
        private int currentDepth = 0;

        internal JsonSerializer(bool SerializeNulls, bool SerializeZeros)
        {
            this.serializeNulls = SerializeNulls;
			this.serializeZeros = SerializeZeros;
        }

        internal string ConvertToJSON(JsonValue obj)
        {
            WriteValue(obj);
            return output.ToString();
        }

        private void WriteValue(JsonValue obj)
        {
            switch (obj.type)
            {
                case JsonType.None:
                case JsonType.Null:
                    output.Append("null");
                    break;
                case JsonType.Boolean:
                    output.Append(((bool)obj.store) ? "true" : "false");
                    break;
                case JsonType.Int:
                    output.Append(Convert.ToString((int)obj.store));
                    break;
                case JsonType.Long:
                    output.Append(Convert.ToString((long)obj.store));
                    break;
                case JsonType.Double:
                    output.Append(Convert.ToString((double)obj.store));
                    break;
                case JsonType.String:
                    WriteString((string)obj.store);
                    break;
                case JsonType.Object:
                    WriteObject(obj);
                    break;
                case JsonType.Array:
                    WriteArray(obj);
                    break;
            }
        }

        private void WriteObject(JsonValue obj)
        {
            currentDepth++;
            if (currentDepth > MAX_DEPTH)
                throw new JsonException("Serializer encountered maximum depth of " + MAX_DEPTH);
            output.Append('{');

            bool append = false;
            foreach (KeyValuePair<string, JsonValue> kv in obj.store as Dictionary<string, JsonValue>)
            {
                if (kv.Value.type == JsonType.None 
				    || (serializeNulls == false && kv.Value.type == JsonType.Null)
				    || (serializeZeros == false && kv.Value.IsZero()))
                    continue;
				
				if (append)
                    output.Append(',');
				
                WritePair(kv.Key, kv.Value);
                append = true;
            }

            currentDepth--;
            output.Append('}');
            currentDepth--;
        }

        private void WriteArray(JsonValue obj)
        {
            currentDepth++;
            if (currentDepth > MAX_DEPTH)
                throw new JsonException("Serializer encountered maximum depth of " + MAX_DEPTH);
            output.AppendLine("[");

            bool append = false;
            foreach (JsonValue v in obj.store as List<JsonValue>)
            {
                if (append)
                    output.AppendLine(",");

                if (v.type == JsonType.None || (v.type == JsonType.Null && serializeNulls == false))
                    append = false;
                else
                {
                    WriteValue(v);
                    append = true;
                }
            }

            currentDepth--;
            output.AppendLine("]");
            currentDepth--;
        }

        private void WritePairFast(string name, string value)
        {
            if ((value == null) && serializeNulls == false)
                return;

            WriteStringFast(name);

            output.Append(':');

            WriteStringFast(value);
        }

        private void WritePair(string name, JsonValue value)
        {
            if ((value.type == JsonType.None || value.type == JsonType.Null) && serializeNulls == false)
                return;

            WriteStringFast(name);

            output.Append(':');

            WriteValue(value);
        }

        private void WriteStringFast(string s)
        {
            output.Append('\"');
            output.Append(s);
            output.Append('\"');
        }

        private void WriteString(string s)
        {
            output.Append('\"');

            int runIndex = -1;

            for (int index = 0; index < s.Length; ++index)
            {
                char c = s[index];

                if (c >= ' ' && c < 128 && c != '\"' && c != '\\')
                {
                    if (runIndex == -1)
                    {
                        runIndex = index;
                    }

                    continue;
                }

                if (runIndex != -1)
                {
                    output.Append(s, runIndex, index - runIndex);
                    runIndex = -1;
                }

                switch (c)
                {
                    case '\t': output.Append("\\t"); break;
                    case '\r': output.Append("\\r"); break;
                    case '\n': output.Append("\\n"); break;
                    case '"':
                    case '\\': output.Append('\\'); output.Append(c); break;
                    default:
                        output.Append("\\u");
                        output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
                        break;
                }
            }

            if (runIndex != -1)
            {
                output.Append(s, runIndex, s.Length - runIndex);
            }

            output.Append('\"');
        }
    }
}

