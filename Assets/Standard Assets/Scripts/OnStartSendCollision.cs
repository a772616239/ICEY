using System;
using UnityEngine;

public class OnStartSendCollision : MonoBehaviour
{
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.effectSettings.OnCollisionHandler(new CollisionInfo());
		this.isInitialized = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.effectSettings.OnCollisionHandler(new CollisionInfo());
		}
	}

	private EffectSettings effectSettings;

	private bool isInitialized;
}
