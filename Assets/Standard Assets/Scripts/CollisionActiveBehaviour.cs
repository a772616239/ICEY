using System;
using UnityEngine;

public class CollisionActiveBehaviour : MonoBehaviour
{
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.IsReverse)
		{
			this.effectSettings.RegistreInactiveElement(base.gameObject, this.TimeDelay);
			base.gameObject.SetActive(false);
		}
		else
		{
			this.effectSettings.RegistreActiveElement(base.gameObject, this.TimeDelay);
		}
		if (this.IsLookAt)
		{
			this.effectSettings.CollisionEnter += this.effectSettings_CollisionEnter;
		}
	}

	private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		base.transform.LookAt(this.effectSettings.transform.position + e.Hit.normal);
	}

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

	public bool IsReverse;

	public float TimeDelay;

	public bool IsLookAt;

	private EffectSettings effectSettings;
}
