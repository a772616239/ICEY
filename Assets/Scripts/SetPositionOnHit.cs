using System;
using UnityEngine;

public class SetPositionOnHit : BaseBehaviour
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
		if (this.effectSettings == null)
		{
			UnityEngine.Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
	}

	private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		Vector3 normalized = (this.tRoot.position + Vector3.Normalize(e.Hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f)).normalized;
		base.transform.position = e.Hit.point - normalized * this.OffsetPosition;
	}

	private void Update()
	{
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			this.effectSettings.CollisionEnter += this.effectSettings_CollisionEnter;
		}
	}

	private void OnDisable()
	{
		base.transform.position = Vector3.zero;
	}

	public float OffsetPosition;

	private EffectSettings effectSettings;

	private Transform tRoot;

	private bool isInitialized;
}
