using System;
using System.IO;

namespace EngineCore
{
	public interface IBinaryable
	{
		void WriterByte(BinaryWriter writer);
		void ReadByte(BinaryReader reader);
	}
}

