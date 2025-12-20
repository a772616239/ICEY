using System;
using UnityEngine;

public class PlatformMaterial : BaseBehaviour
{
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component && component.material == null)
		{
			component.material = this._matPS4;
		}
	}

	[SerializeField]
	private Material _matPS4;
}
