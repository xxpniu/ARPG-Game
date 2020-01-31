using System;
using System.Collections.Generic;

namespace org.vxwo.csharp.json
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonValue
    {
        internal JsonType type;
        internal object store;

        /// <summary>
        /// 
        /// </summary>
        public JsonValue()
        {
            type = JsonType.None;
            store = null;
        }

        internal JsonValue(JsonType type, object obj)
        {
            this.type = type;
            if(this.type == JsonType.Array && obj == null)
                this.store = new List<JsonValue>();
	    else
                this.store = obj;
        }
		
		internal bool IsZero()
		{
			if(IsInt())
				return (int)store==0;
			if(IsLong())
				return (long)store==0;
			if(IsDouble())
				return (double)store==0.0f;
			return false;
		}

        /// <summary>
        /// clear
        /// </summary>
        public void Clear()
        {
            type = JsonType.None;
            store = null;
        }

        private Dictionary<string, JsonValue> EnsureObject()
        {
            if (type != JsonType.None && type != JsonType.Object)
                throw new Exception("JsonValue not a object");

            if (type == JsonType.None)
            {
                type = JsonType.Object;
                store = new Dictionary<string, JsonValue>();
            }
            return store as Dictionary<string, JsonValue>;
        }

        /// <summary>
        /// get member
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonValue this[string name]
        {
            get
            {
                JsonValue result;
                Dictionary<string, JsonValue> obj = EnsureObject();
                if (!obj.TryGetValue(name, out result))
                {
                    result = new JsonValue();
                    obj.Add(name, result);
                }
                return result;
            }

            set
            {
                if(value == null)
                    value = new JsonValue(JsonType.Null, null);
                EnsureObject().Add(name, value);
            }
        }

        /// <summary>
        /// is member
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsMember(string name)
        {
            return EnsureObject().ContainsKey(name);
        }

        /// <summary>
        /// delete member
        /// </summary>
        /// <param name="name"></param>
        public void RemoveMember(string name)
        {
            EnsureObject().Remove(name);
        }

        private List<JsonValue> EnsureArray()
        {
            if (type != JsonType.None && type != JsonType.Array)
                throw new Exception("JsonValue not a array");

            if (type == JsonType.None)
            {
                type = JsonType.Array;
                store = new List<JsonValue>();
            }
            return store as List<JsonValue>;
        }
        /// <summary>
        /// array count
        /// </summary>
        public int Count
        {
            get
            {
                if (type != JsonType.Array)
                    return 0;
                return EnsureArray().Count;
            }
        }

        /// <summary>
        /// get index if it have arrary
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JsonValue GetAt(int index)
        {
            return EnsureArray()[index];
        }
        /// <summary>
        /// append a jsonvalue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public JsonValue Append(JsonValue value)
        {
            if(value != null)
            	EnsureArray().Add(value);
            return this;
        }
        
        /// <summary>
        /// is null?
        /// </summary>
        /// <returns></returns>
        public bool IsNull()
        {
            return type == JsonType.Null || type == JsonType.None;
        }
	/// <summary>
    ///  type of boolean
    /// </summary>
    /// <returns></returns>
        public bool IsBoolean()
        {
            return type == JsonType.Boolean;
        }
        /// <summary>
        /// type of int 
        /// </summary>
        /// <returns></returns>
        public bool IsInt()
        {
            return type == JsonType.Int;
        }
        /// <summary>
        /// type of long
        /// </summary>
        /// <returns></returns>
        public bool IsLong()
        {
            return type == JsonType.Long;
        }
        /// <summary>
        /// type of double
        /// </summary>
        /// <returns></returns>
        public bool IsDouble()
        {
            return type == JsonType.Double;
        }
        /// <summary>
        /// is type of string 
        /// </summary>
        /// <returns></returns>
        public bool IsString()
        {
            return type == JsonType.String;
        }
        /// <summary>
        ///  as to boolean
        /// </summary>
        /// <returns></returns>
        public bool AsBoolean()
        {
            bool value = false;
            if (IsBoolean())
                value = (bool)store;
            else if (IsString())
                value = bool.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a bool");
            return value;
        }
        /// <summary>
        ///  as to int
        /// </summary>
        /// <returns></returns>
        public int AsInt()
        {
            int value = 0;
            if (IsInt())
                value = (int)store;
            else if (IsString())
                value = int.Parse((string)store);
            else if (IsLong())
                value = (int)(long)store;
            else if (IsDouble())
                value = (int)(double)store;
            else
                throw new Exception("JsonValue cannot convert to a int");
            return value;
        }
        /// <summary>
        /// as to long
        /// </summary>
        /// <returns></returns>
        public long AsLong()
        {
            long value = 0;
            if (IsLong())
                value = (long)store;
            else if (IsString())
                value = long.Parse((string)store);
            else if (IsInt())
                value = (long)(int)store;
            else if (IsDouble())
                value = (long)(double)store;
            else
                throw new Exception("JsonValue cannot convert to a long");
            return value;
        }
        /// <summary>
        /// as to double
        /// </summary>
        /// <returns></returns>
        public double AsDouble()
        {
            double value = 0;
            if (IsDouble())
                value = (double)store;
            else if (IsString())
                value = double.Parse((string)store);
            else if (IsInt())
                value = (double)(int)store;
            else if (IsLong())
                value = (double)(long)store;
            else
                throw new Exception("JsonValue cannot convert to a double");
            return value;
        }
        /// <summary>
        ///  as to string
        /// </summary>
        /// <returns></returns>
        public string AsString()
        {
            return IsString() ? (string)store : Convert.ToString(store);
        }

        /// <summary>
        /// boolean to json value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue(bool value)
        {
            return new JsonValue(JsonType.Boolean, value);
        }
        /// <summary>
        /// int to json value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue(int value)
        {
            return new JsonValue(JsonType.Int, value);
        }
        /// <summary>
        /// long to json value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue(long value)
        {
            return new JsonValue(JsonType.Long, value);
        }
        /// <summary>
        /// double to json value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue(double value)
        {
            return new JsonValue(JsonType.Double, value);
        }
        /// <summary>
        /// string to json value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue(string value)
        {
            return new JsonValue(JsonType.String, value);
        }

    }
}
