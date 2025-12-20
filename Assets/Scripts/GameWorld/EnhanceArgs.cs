using System;

namespace GameWorld
{
	public class EnhanceArgs : EventArgs
	{
		public EnhanceArgs(string name, int upToLevel)
		{
			this.Name = name;
			this.UpToLevel = upToLevel;
		}

		public string Name;

		public int UpToLevel;
	}
}
