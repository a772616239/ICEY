using System;
using System.Diagnostics;
using UnityEngine;

public class MoveOnGround : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<CollisionInfo> OnCollision;

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
		this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	private void InitDefaultVariables()
	{
		foreach (ParticleSystem particleSystem in this.particles)
		{
			particleSystem.Stop();
		}
		this.isFinished = false;
		this.tTarget = this.effectSettings.Target.transform;
		if (this.IsRootMove)
		{
			this.tRoot = this.effectSettings.transform;
		}
		else
		{
			this.tRoot = base.transform.parent;
			this.tRoot.localPosition = Vector3.zero;
		}
		this.targetPos = this.tRoot.position + Vector3.Normalize(this.tTarget.position - this.tRoot.position) * this.effectSettings.MoveDistance;
		RaycastHit raycastHit;
		Physics.Raycast(this.tRoot.position, Vector3.down, out raycastHit);
		this.tRoot.position = raycastHit.point;
		foreach (ParticleSystem particleSystem2 in this.particles)
		{
			particleSystem2.Play();
		}
	}

	private void Update()
	{
		if (this.tTarget == null || this.isFinished)
		{
			return;
		}
		Vector3 position = this.tRoot.position;
		RaycastHit raycastHit;
		Physics.Raycast(new Vector3(position.x, 0.5f, position.z), Vector3.down, out raycastHit);
		this.tRoot.position = raycastHit.point;
		position = this.tRoot.position;
		Vector3 vector = (!this.effectSettings.IsHomingMove) ? this.targetPos : this.tTarget.position;
		Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
		if (Vector3.Distance(new Vector3(position.x, 0f, position.z), vector2) <= this.effectSettings.ColliderRadius)
		{
			this.effectSettings.OnCollisionHandler(new CollisionInfo());
			this.isFinished = true;
		}
		this.tRoot.position = Vector3.MoveTowards(position, vector2, this.effectSettings.MoveSpeed * Time.deltaTime);
	}

	public bool IsRootMove = true;

	private EffectSettings effectSettings;

	private Transform tRoot;

	private Transform tTarget;

	private Vector3 targetPos;

	private bool isInitialized;

	private bool isFinished;

	private ParticleSystem[] particles;
}
