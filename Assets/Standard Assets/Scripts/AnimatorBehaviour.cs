using System;
using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
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
		this.oldSpeed = this.anim.speed;
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
		this.isInitialized = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.anim.speed = this.oldSpeed;
		}
	}

	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.anim.speed = 0f;
	}

	private void Update()
	{
	}

	public Animator anim;

	private EffectSettings effectSettings;

	private bool isInitialized;

	private float oldSpeed;
}
