using System;
using System.Reflection;
using System.Collections;

namespace org.vxwo.csharp.json
{
	class JsonObjParser
	{		
		private object root;
		
		internal JsonObjParser (object obj)
		{
			this.root = obj;
		}

		internal JsonValue Decode ()
		{
			return ParseValue (root);
		}
		
		private JsonValue ParseValue (object obj)
		{
			if (obj == null)
				return new JsonValue (JsonType.Null, null);
			
			Type type = obj.GetType ();
			
			if (type.IsArray) {
				JsonValue child, result = new JsonValue (JsonType.Array, null);
				foreach (object sub in (Array)obj) {
					child = ParseValue (sub);
					if (child != null)
						result.Append (child);
				}
				return result;
			}
			if (type.Name.Equals ("Char"))
				return new JsonValue (JsonType.Int, Convert.ToInt32(obj));
			if (type.Name.Equals ("Byte"))
				return new JsonValue (JsonType.Int, Convert.ToInt32(obj));
			if (type.Name.Equals ("Boolean"))
				return new JsonValue (JsonType.Boolean, obj);
			if (type.Name.Equals ("Int32"))
				return new JsonValue (JsonType.Int, obj);
			if (type.Name.Equals ("Int64"))
				return new JsonValue (JsonType.Long, obj);
			if (type.Name.Equals ("Single"))
				return new JsonValue (JsonType.Double, (double)(float)obj);
			if (type.Name.Equals ("Double"))
				return new JsonValue (JsonType.Double, obj);
			if (type.Name.Equals ("String"))
				return new JsonValue (JsonType.String, obj);
			if (type.Name.Equals ("DateTime"))
				return new JsonValue (JsonType.String, string.Format("{0:yyyy-MM-dd HH:mm:ss}",(DateTime)obj));
			
			if (type.IsEnum)
				return new JsonValue (JsonType.Int, Convert.ToInt32 (obj));
			
			if (type.Name.Equals("List`1")) {
				JsonValue child, result = new JsonValue (JsonType.Array, null);
				foreach (object sub in (IList)obj) {
					child = ParseValue (sub);
					if (child != null)
						result.Append (child);
				}
				return result;
			}
			
			if (type.IsClass) {
				JsonValue child, result = new JsonValue ();
				foreach(PropertyInfo info in type.GetProperties())
				{
					if(!info.CanRead)
						continue;
					bool ignore = false;
					string name = info.Name;
					foreach(Attribute attr in info.GetCustomAttributes(true))
					{
						if(attr is JsonIgnore)
						{
							ignore=true;
							break;
						}
						else if(attr is JsonName)
							name = ((JsonName)attr).GetName();
					}
					if(ignore)
						continue;
					child = ParseValue (info.GetValue(obj, null));
					if (child != null)
						result [name] = child;
				}
				foreach (FieldInfo info in type.GetFields()) {
					if(!info.IsPublic)
						continue;
					if(info.IsLiteral)
						continue;
					bool ignore = false;
					string name = info.Name;
					foreach(Attribute attr in info.GetCustomAttributes(true))
					{
						if(attr is JsonIgnore)
						{
							ignore=true;
							break;
						}
						else if(attr is JsonName)
							name = ((JsonName)attr).GetName();
					}
					if(ignore)
						continue;
					child = ParseValue (info.GetValue (obj));
					if (child != null)
						result [name] = child;
				}
				return result;
			}
			
			throw new JsonException ("JsonObjParser not support type: " + type.Name);
		}
	}
}
