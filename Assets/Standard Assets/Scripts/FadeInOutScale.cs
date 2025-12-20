using System;
using UnityEngine;

public class FadeInOutScale : MonoBehaviour
{
	private void Start()
	{
		this.t = base.transform;
		this.oldScale = this.t.localScale;
		this.isInitialized = true;
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
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

	public void InitDefaultVariables()
	{
		if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision)
		{
			this.t.localScale = this.oldScale;
			this.canUpdate = false;
		}
		else
		{
			this.t.localScale = Vector3.zero;
			this.canUpdate = true;
		}
		this.updateTime = true;
		this.time = 0f;
		this.oldSin = 0f;
		this.isCollisionEnter = false;
	}

	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		this.canUpdate = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	private void Update()
	{
		if (!this.canUpdate)
		{
			return;
		}
		if (this.updateTime)
		{
			this.time = Time.time;
			this.updateTime = false;
		}
		float num = Mathf.Sin((Time.time - this.time) / this.Speed);
		float num2;
		if (this.oldSin > num)
		{
			this.canUpdate = false;
			num2 = this.MaxScale;
		}
		else
		{
			num2 = num * this.MaxScale;
		}
		if (this.FadeInOutStatus == FadeInOutStatus.In)
		{
			if (num2 < this.MaxScale)
			{
				this.t.localScale = new Vector3(this.oldScale.x * num2, this.oldScale.y * num2, this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = new Vector3(this.MaxScale, this.MaxScale, this.MaxScale);
			}
		}
		if (this.FadeInOutStatus == FadeInOutStatus.Out)
		{
			if (num2 > 0f)
			{
				this.t.localScale = new Vector3(this.MaxScale * this.oldScale.x - this.oldScale.x * num2, this.MaxScale * this.oldScale.y - this.oldScale.y * num2, this.MaxScale * this.oldScale.z - this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = Vector3.zero;
			}
		}
		if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision && this.isCollisionEnter)
		{
			if (num2 > 0f)
			{
				this.t.localScale = new Vector3(this.MaxScale * this.oldScale.x - this.oldScale.x * num2, this.MaxScale * this.oldScale.y - this.oldScale.y * num2, this.MaxScale * this.oldScale.z - this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = Vector3.zero;
			}
		}
		this.oldSin = num;
	}

	public FadeInOutStatus FadeInOutStatus;

	public float Speed = 1f;

	public float MaxScale = 2f;

	private Vector3 oldScale;

	private float time;

	private float oldSin;

	private bool updateTime = true;

	private bool canUpdate = true;

	private Transform t;

	private EffectSettings effectSettings;

	private bool isInitialized;

	private bool isCollisionEnter;
}
