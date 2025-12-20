using System;
using UnityEngine;

public class UpRisingDistort : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (base.transform.parent == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
