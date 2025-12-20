using System;
using System.Diagnostics;
using UnityEngine;

public class ClothGridCollisionBehaviour : MonoBehaviour
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
		this.tRoot = this.effectSettings.transform;
		this.InitDefaultVariables();
	}

	private void InitDefaultVariables()
	{
		this.tTarget = this.effectSettings.Target.transform;
		if (this.IsLookAt)
		{
			this.tRoot.LookAt(this.tTarget);
		}
		Vector3 vector = this.CenterPoint();
		this.targetPos = vector + Vector3.Normalize(this.tTarget.position - vector) * this.effectSettings.MoveDistance;
		Vector3 force = Vector3.Normalize(this.tTarget.position - vector) * this.effectSettings.MoveSpeed / 100f;
		for (int i = 0; i < this.AttachedPoints.Length; i++)
		{
			GameObject gameObject = this.AttachedPoints[i];
			Rigidbody component = gameObject.GetComponent<Rigidbody>();
			component.useGravity = false;
			component.AddForce(force, ForceMode.Impulse);
			gameObject.SetActive(true);
		}
	}

	private Vector3 CenterPoint()
	{
		return (base.transform.TransformPoint(this.AttachedPoints[0].transform.localPosition) + base.transform.TransformPoint(this.AttachedPoints[2].transform.localPosition)) / 2f;
	}

	private void Update()
	{
		if (this.tTarget == null || this.onCollision)
		{
			return;
		}
		Vector3 vector = this.CenterPoint();
		RaycastHit hit = default(RaycastHit);
		float num = Vector3.Distance(vector, this.targetPos);
		UnityEngine.Debug.DrawLine(vector, this.targetPos);
		float num2 = this.effectSettings.MoveSpeed * Time.deltaTime;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num <= this.effectSettings.ColliderRadius)
		{
			this.DeactivateAttachedPoints(hit);
		}
		Vector3 normalized = (this.targetPos - vector).normalized;
		if (Physics.Raycast(vector, normalized, out hit, num2 + this.effectSettings.ColliderRadius))
		{
			this.targetPos = hit.point - normalized * this.effectSettings.ColliderRadius;
			this.DeactivateAttachedPoints(hit);
		}
	}

	private void DeactivateAttachedPoints(RaycastHit hit)
	{
		for (int i = 0; i < this.AttachedPoints.Length; i++)
		{
			GameObject gameObject = this.AttachedPoints[i];
			gameObject.SetActive(false);
		}
		CollisionInfo e = new CollisionInfo
		{
			Hit = hit
		};
		this.effectSettings.OnCollisionHandler(e);
		if (hit.transform != null)
		{
			ShieldCollisionBehaviour component = hit.transform.GetComponent<ShieldCollisionBehaviour>();
			if (component != null)
			{
				component.ShieldCollisionEnter(e);
			}
		}
		this.onCollision = true;
	}

	public GameObject[] AttachedPoints;

	public bool IsLookAt;

	private EffectSettings effectSettings;

	private Transform tRoot;

	private Transform tTarget;

	private Vector3 targetPos;

	private bool onCollision;
}
