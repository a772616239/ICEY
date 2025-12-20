using System;
using UnityEngine;

namespace GameWorld
{
	public class HitWallArgs : EventArgs
	{
		public HitWallArgs(GameObject who)
		{
			this.who = who;
		}

		public GameObject who;
	}
}
