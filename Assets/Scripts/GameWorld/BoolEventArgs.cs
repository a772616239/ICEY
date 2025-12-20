using System;

namespace GameWorld
{
	public class BoolEventArgs : EventArgs
	{
		public BoolEventArgs(bool success)
		{
			this.Success = success;
		}

		public readonly bool Success;
	}
}
