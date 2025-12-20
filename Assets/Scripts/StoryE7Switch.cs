using System;
using UnityEngine;

public class StoryE7Switch : BaseBehaviour
{
	private bool E7Finish
	{
		get
		{
			return SaveStorage.Get("E7_Finish", false);
		}
	}

	private void Start()
	{
		if (this.E7Finish)
		{
			this._gate.openType = SceneGate.OpenType.None;
		}
	}

	[SerializeField]
	private SceneGate _gate;
}
