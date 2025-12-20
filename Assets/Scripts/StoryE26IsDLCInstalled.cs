using System;
using UnityEngine;

public class StoryE26IsDLCInstalled : BaseBehaviour
{
	private void Start()
	{
		this._gate.gameObject.SetActive(true);
		base.gameObject.SetActive(true);
	}

	[SerializeField]
	private SceneGate _gate;
}
