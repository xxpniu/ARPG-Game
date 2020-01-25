using System;
using System.Text;

namespace org.vxwo.csharp.json
{
	class JsonParser
    {
        enum Token
        {
            None = -1,           // Used to denote no Lookahead available
            Curly_Open,
            Curly_Close,
            Squared_Open,
            Squared_Close,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null
        }

        private char[] json;
        private StringBuilder s = new StringBuilder();
        private Token lookAheadToken = Token.None;
        private int index;

        internal JsonParser(string json)
        {
            this.json = json.ToCharArray();
        }

        public JsonValue Decode()
        {
            return ParseValue();
        }

        private JsonValue ParseObject()
        {
            JsonValue obj = new JsonValue();

            ConsumeToken(); // {

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Curly_Close:
                        ConsumeToken();
                        return obj;

                    default:
                        {

                            // name
                            string name = ParseString();

                            // :
                            if (NextToken() != Token.Colon)
                            {
                                throw new JsonException("Expected colon at index " + index);
                            }

                            // value
                            obj[name] = ParseValue();
                        }
                        break;
                }
            }
        }


        private JsonValue ParseArray()
        {
            JsonValue array = new JsonValue(JsonType.Array, null);

            ConsumeToken(); // [

            while (true)
            {
                switch (LookAhead())
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Squared_Close:
                        ConsumeToken();
                        return array;

                    default:
                        {
                            array.Append(ParseValue());
                        }
                        break;
                }
            }
        }

        private JsonValue ParseValue()
        {
            switch (LookAhead())
            {
                case Token.Number:
                    return ParseNumber();

                case Token.String:
                    return (JsonValue)ParseString();

                case Token.Curly_Open:
                    return ParseObject();

                case Token.Squared_Open:
                    return ParseArray();

                case Token.True:
                    ConsumeToken();
                    return (JsonValue)true;

                case Token.False:
                    ConsumeToken();
                    return (JsonValue)false;

                case Token.Null:
                    ConsumeToken();
                    return new JsonValue(JsonType.Null, null);
            }

            throw new JsonException("Unrecognized token at index" + index);
        }

        private string ParseString()
        {
            ConsumeToken(); // "

            s.Length = 0;

            int runIndex = -1;

            while (index < json.Length)
            {
                char c = json[index++];

                if (c == '"')
                {
                    if (runIndex != -1)
                    {
                        if (s.Length == 0)
                            return new string(json, runIndex, index - runIndex - 1);

                        s.Append(json, runIndex, index - runIndex - 1);
                    }
                    return s.ToString();
                }

                if (c != '\\')
                {
                    if (runIndex == -1)
                        runIndex = index - 1;

                    continue;
                }

                if (index == json.Length) break;

                if (runIndex != -1)
                {
                    s.Append(json, runIndex, index - runIndex - 1);
                    runIndex = -1;
                }

                switch (json[index++])
                {
                    case '"':
                        s.Append('"');
                        break;

                    case '\\':
                        s.Append('\\');
                        break;

                    case '/':
                        s.Append('/');
                        break;

                    case 'b':
                        s.Append('\b');
                        break;

                    case 'f':
                        s.Append('\f');
                        break;

                    case 'n':
                        s.Append('\n');
                        break;

                    case 'r':
                        s.Append('\r');
                        break;

                    case 't':
                        s.Append('\t');
                        break;

                    case 'u':
                        {
                            int remainingLength = json.Length - index;
                            if (remainingLength < 4) break;

                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint = ParseUnicode(json[index], json[index + 1], json[index + 2], json[index + 3]);
                            s.Append((char)codePoint);

                            // skip 4 chars
                            index += 4;
                        }
                        break;
                }
            }

            throw new JsonException("Unexpectedly reached end of string");
        }

        private uint ParseSingleChar(char c1, uint multipliyer)
        {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9')
                p1 = (uint)(c1 - '0') * multipliyer;
            else if (c1 >= 'A' && c1 <= 'F')
                p1 = (uint)((c1 - 'A') + 10) * multipliyer;
            else if (c1 >= 'a' && c1 <= 'f')
                p1 = (uint)((c1 - 'a') + 10) * multipliyer;
            return p1;
        }

        private uint ParseUnicode(char c1, char c2, char c3, char c4)
        {
            uint p1 = ParseSingleChar(c1, 0x1000);
            uint p2 = ParseSingleChar(c2, 0x100);
            uint p3 = ParseSingleChar(c3, 0x10);
            uint p4 = ParseSingleChar(c4, 1);

            return p1 + p2 + p3 + p4;
        }

        private JsonValue ParseNumber()
        {
            ConsumeToken();

            // Need to start back one place because the first digit is also a token and would have been consumed
            int startIndex = index - 1;

            do
            {
                char c = json[index];

                if ((c >= '0' && c <= '9') || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E')
                {
                    if (++index == json.Length) throw new JsonException("Unexpected end of string whilst parsing number");
                    continue;
                }

                break;
            } while (true);

            string number = new string(json, startIndex, index - startIndex);

            JsonValue result = null;
            if (number.IndexOf('.') != -1 ||
                 number.IndexOf('e') != -1 ||
                 number.IndexOf('E') != -1)
            {
                double value_double;
                if (double.TryParse(number, out value_double))
                    result = (JsonValue)value_double;
            }
            if (result == null)
            {
                int value_int;
                if (int.TryParse(number, out value_int))
                    result = (JsonValue)value_int;
            }
            if (result == null)
            {
                long value_long;
                if (long.TryParse(number, out value_long))
                    result = (JsonValue)value_long;
            }
            if (result == null)
                result = (JsonValue)0;
            return result;
        }

        private Token LookAhead()
        {
            if (lookAheadToken != Token.None) return lookAheadToken;

            return lookAheadToken = NextTokenCore();
        }

        private void ConsumeToken()
        {
            lookAheadToken = Token.None;
        }

        private Token NextToken()
        {
            Token result = lookAheadToken != Token.None ? lookAheadToken : NextTokenCore();

            lookAheadToken = Token.None;

            return result;
        }

        private Token NextTokenCore()
        {
            char c;

            // Skip head whitespace
            if (index == 0) {
                do
                {
                    c = json[index];

                    if (c == '{') break;
                    if (c < (char)0x80 && c != ' ' && c != '\t' && c != '\n' && c != '\r') break;

                } while (++index < json.Length);

                if (index == json.Length)
                {
                    throw new JsonException("Reached end of string unexpectedly");
                }
            }
            // Skip past whitespace
            do
            {
                c = json[index];

                if (c > ' ') break;
                if (c != ' ' && c != '\t' && c != '\n' && c != '\r') break;

            } while (++index < json.Length);

            if (index == json.Length)
            {
                throw new JsonException("Reached end of string unexpectedly");
            }

            c = json[index];

            index++;

            switch (c)
            {
                case '{':
                    return Token.Curly_Open;

                case '}':
                    return Token.Curly_Close;

                case '[':
                    return Token.Squared_Open;

                case ']':
                    return Token.Squared_Close;

                case ',':
                    return Token.Comma;

                case '"':
                    return Token.String;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                case '+':
                case '.':
                    return Token.Number;

                case ':':
                    return Token.Colon;

                case 'f':
                    if (json.Length - index >= 4 &&
                        json[index + 0] == 'a' &&
                        json[index + 1] == 'l' &&
                        json[index + 2] == 's' &&
                        json[index + 3] == 'e')
                    {
                        index += 4;
                        return Token.False;
                    }
                    break;

                case 't':
                    if (json.Length - index >= 3 &&
                        json[index + 0] == 'r' &&
                        json[index + 1] == 'u' &&
                        json[index + 2] == 'e')
                    {
                        index += 3;
                        return Token.True;
                    }
                    break;

                case 'n':
                    if (json.Length - index >= 3 &&
                        json[index + 0] == 'u' &&
                        json[index + 1] == 'l' &&
                        json[index + 2] == 'l')
                    {
                        index += 3;
                        return Token.Null;
                    }
                    break;

            }

            throw new JsonException("Could not find token at index " + --index);
        }
    }
}

