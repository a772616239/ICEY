using System;
using UnityEngine;

public class MoveEffect : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DestroyEffect()
	{
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
	}
}
