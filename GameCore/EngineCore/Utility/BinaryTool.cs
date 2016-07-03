using System;

namespace EngineCore.Utility
{
	public class BinaryTool
	{
		public static byte[] WriteBinary<T>(T data) where T:IBinaryable
		{
			using (var mem = new System.IO.MemoryStream ()) {
				using (var wr = new System.IO.BinaryWriter (mem)) {
					data.WriterByte (wr);
				}

				return mem.ToArray ();
			}
		}

		public static T ReadBinary<T>(byte[] data) where T : IBinaryable, new()
		{
			var temp = new T ();
			using (var mem = new System.IO.MemoryStream (data)) {
				using (var br = new System.IO.BinaryReader (mem)) {
				
					temp.ReadByte (br);
				}
			}

			return temp;
		}

	   
	}
}

