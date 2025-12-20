using System;
using UnityEngine;

public class KillFlash : BaseBehaviour
{
	private void Start()
	{
		base.transform.localScale = new Vector3(R.Player.Transform.localScale.x, 1f, 1f);
	}

	private void Update()
	{
	}
}
