using System;
using UnityEngine;

public class LineRendererBehaviour : MonoBehaviour
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
			UnityEngine.Debug.Log("Prefab root have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.line = base.GetComponent<LineRenderer>();
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	private void InitializeDefault()
	{
		base.GetComponent<Renderer>().material.SetFloat("_Chanel", (float)this.currentShaderIndex);
		this.currentShaderIndex++;
		if (this.currentShaderIndex == 3)
		{
			this.currentShaderIndex = 0;
		}
		this.line.SetPosition(0, this.tRoot.position);
		if (this.IsVertical)
		{
			if (Physics.Raycast(this.tRoot.position, Vector3.down, out this.hit))
			{
				this.line.SetPosition(1, this.hit.point);
				if (this.StartGlow != null)
				{
					this.StartGlow.transform.position = this.tRoot.position;
				}
				if (this.HitGlow != null)
				{
					this.HitGlow.transform.position = this.hit.point;
				}
				if (this.GoLight != null)
				{
					this.GoLight.transform.position = this.hit.point + new Vector3(0f, this.LightHeightOffset, 0f);
				}
				if (this.Particles != null)
				{
					this.Particles.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
				if (this.Explosion != null)
				{
					this.Explosion.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
			}
		}
		else
		{
			if (this.effectSettings.Target != null)
			{
				this.tTarget = this.effectSettings.Target.transform;
			}
			else if (!this.effectSettings.UseMoveVector)
			{
				UnityEngine.Debug.Log("You must setup the the target or the motion vector");
			}
			Vector3 vector;
			if (!this.effectSettings.UseMoveVector)
			{
				vector = (this.tTarget.position - this.tRoot.position).normalized;
			}
			else
			{
				vector = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
			}
			Vector3 a = this.tRoot.position + vector * this.effectSettings.MoveDistance;
			if (Physics.Raycast(this.tRoot.position, vector, out this.hit, this.effectSettings.MoveDistance + 1f, this.effectSettings.LayerMask))
			{
				a = (this.tRoot.position + Vector3.Normalize(this.hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f)).normalized;
			}
			this.line.SetPosition(1, this.hit.point - this.effectSettings.ColliderRadius * a);
			Vector3 vector2 = this.hit.point - a * this.ParticlesHeightOffset;
			if (this.StartGlow != null)
			{
				this.StartGlow.transform.position = this.tRoot.position;
			}
			if (this.HitGlow != null)
			{
				this.HitGlow.transform.position = vector2;
			}
			if (this.GoLight != null)
			{
				this.GoLight.transform.position = this.hit.point - a * this.LightHeightOffset;
			}
			if (this.Particles != null)
			{
				this.Particles.transform.position = vector2;
			}
			if (this.Explosion != null)
			{
				this.Explosion.transform.position = vector2;
				this.Explosion.transform.LookAt(vector2 + this.hit.normal);
			}
		}
		CollisionInfo e = new CollisionInfo
		{
			Hit = this.hit
		};
		this.effectSettings.OnCollisionHandler(e);
		if (this.hit.transform != null)
		{
			ShieldCollisionBehaviour component = this.hit.transform.GetComponent<ShieldCollisionBehaviour>();
			if (component != null)
			{
				component.ShieldCollisionEnter(e);
			}
		}
	}

	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	public bool IsVertical;

	public float LightHeightOffset = 0.3f;

	public float ParticlesHeightOffset = 0.2f;

	public float TimeDestroyLightAfterCollision = 4f;

	public float TimeDestroyThisAfterCollision = 4f;

	public float TimeDestroyRootAfterCollision = 4f;

	public GameObject EffectOnHitObject;

	public GameObject Explosion;

	public GameObject StartGlow;

	public GameObject HitGlow;

	public GameObject Particles;

	public GameObject GoLight;

	private EffectSettings effectSettings;

	private Transform tRoot;

	private Transform tTarget;

	private bool isInitializedOnStart;

	private LineRenderer line;

	private int currentShaderIndex;

	private RaycastHit hit;
}
