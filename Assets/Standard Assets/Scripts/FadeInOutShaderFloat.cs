using System;
using UnityEngine;

public class FadeInOutShaderFloat : MonoBehaviour
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
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += this.prefabSettings_CollisionEnter;
		}
		this.InitMaterial();
	}

	public void UpdateMaterial(Material instanceMaterial)
	{
		this.mat = instanceMaterial;
		this.InitMaterial();
	}

	private void InitMaterial()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (base.GetComponent<Renderer>() != null)
		{
			this.mat = base.GetComponent<Renderer>().material;
		}
		else
		{
			LineRenderer component = base.GetComponent<LineRenderer>();
			if (component != null)
			{
				this.mat = component.material;
			}
			else
			{
				Projector component2 = base.GetComponent<Projector>();
				if (component2 != null)
				{
					if (!component2.material.name.EndsWith("(Instance)"))
					{
						component2.material = new Material(component2.material)
						{
							name = component2.material.name + " (Instance)"
						};
					}
					this.mat = component2.material;
				}
			}
		}
		if (this.mat == null)
		{
			return;
		}
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.canStartFadeOut = false;
		this.canStart = false;
		this.isCollisionEnter = false;
		this.oldFloat = 0f;
		this.currentFloat = this.MaxFloat;
		if (this.isIn)
		{
			this.currentFloat = 0f;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		if (this.isStartDelay)
		{
			base.Invoke("SetupStartDelay", this.StartDelay);
		}
		else
		{
			this.canStart = true;
		}
		if (!this.isIn)
		{
			if (!this.FadeOutAfterCollision)
			{
				base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
			}
			this.oldFloat = this.MaxFloat;
		}
	}

	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	private void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		if (this.effectSettings != null && this.UseHideStatus)
		{
			if (!this.effectSettings.IsVisible && this.fadeInComplited)
			{
				this.fadeInComplited = false;
			}
			if (this.effectSettings.IsVisible && this.fadeOutComplited)
			{
				this.fadeOutComplited = false;
			}
		}
		if (this.UseHideStatus)
		{
			if (this.isIn && this.effectSettings != null && this.effectSettings.IsVisible && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.effectSettings != null && !this.effectSettings.IsVisible && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else if (!this.FadeOutAfterCollision)
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.isCollisionEnter && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
	}

	private void FadeIn()
	{
		this.currentFloat = this.oldFloat + Time.deltaTime / this.FadeInSpeed * this.MaxFloat;
		if (this.currentFloat >= this.MaxFloat)
		{
			this.fadeInComplited = true;
			this.currentFloat = this.MaxFloat;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	private void FadeOut()
	{
		this.currentFloat = this.oldFloat - Time.deltaTime / this.FadeOutSpeed * this.MaxFloat;
		if (this.currentFloat <= 0f)
		{
			this.currentFloat = 0f;
			this.fadeOutComplited = true;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	public string PropertyName = "_CutOut";

	public float MaxFloat = 1f;

	public float StartDelay;

	public float FadeInSpeed;

	public float FadeOutDelay;

	public float FadeOutSpeed;

	public bool FadeOutAfterCollision;

	public bool UseHideStatus;

	private Material OwnMaterial;

	private Material mat;

	private float oldFloat;

	private float currentFloat;

	private bool canStart;

	private bool canStartFadeOut;

	private bool fadeInComplited;

	private bool fadeOutComplited;

	private bool previousFrameVisibleStatus;

	private bool isCollisionEnter;

	private bool isStartDelay;

	private bool isIn;

	private bool isOut;

	private EffectSettings effectSettings;

	private bool isInitialized;
}
