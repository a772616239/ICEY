using System;

namespace I2.Loc
{
	[Serializable]
	public class LanguageData
	{
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		public string Name;

		public string Code;

		public byte Flags;

		[NonSerialized]
		public bool Compressed;
	}
}
