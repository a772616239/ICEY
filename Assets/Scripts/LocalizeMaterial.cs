using System;
using UnityEngine;

public class LocalizeMaterial : BaseBehaviour
{
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component)
		{
			if (UILanguage.IsSimplifiedChinese)
			{
				component.material = this._matZhCN;
			}
			else if (UILanguage.IsTraditionalChinese)
			{
				component.material = this._matZhTW;
			}
			else
			{
				component.material = this._mat;
			}
		}
	}

	[SerializeField]
	private Material _mat;

	[SerializeField]
	private Material _matZhCN;

	[SerializeField]
	private Material _matZhTW;
}
