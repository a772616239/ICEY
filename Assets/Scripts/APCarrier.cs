using System;
using UnityEngine;

[AddComponentMenu("Advanced Platformer 2D/Carrier")]
public class APCarrier : BaseBehaviour
{
	public Vector2 ComputeVelocityAt(Vector2 worldPoint)
	{
		Vector3 rhs = (Vector3)worldPoint - base.transform.position;
		return this.m_linearVel + (Vector2)Vector3.Cross(this.m_angularVel, rhs);
	}

	private void Start()
	{
		this.m_prevPos = base.transform.position;
		this.m_prevRot = base.transform.rotation;
		this.m_linearVel = Vector2.zero;
		this.m_angularVel = Vector3.zero;
		this.m_bNextUpdate = false;
		this.m_bUpdate = false;
	}

	private void FixedUpdate()
	{
		if (!this.m_animationMode)
		{
			this.UpdateVelocities();
		}
		else
		{
			if (this.m_bNextUpdate)
			{
				this.m_bNextUpdate = false;
				base.transform.position = this.m_nextPos;
				base.transform.rotation = this.m_nextRot;
			}
			this.UpdateVelocities();
			this.m_bUpdate = true;
		}
	}

	private void Update()
	{
		if (this.m_animationMode && this.m_bUpdate)
		{
			this.m_bUpdate = false;
			this.m_bNextUpdate = true;
			this.m_nextPos = base.transform.position;
			this.m_nextRot = base.transform.rotation;
			base.transform.position = this.m_prevPos;
			base.transform.rotation = this.m_prevRot;
		}
	}

	private void UpdateVelocities()
	{
		this.m_linearVel = ((Vector2)base.transform.position - this.m_prevPos) / Time.fixedDeltaTime;
		float num = Quaternion.Angle(this.m_prevRot, base.transform.rotation);
		Vector3 lhs = this.m_prevRot * Vector3.up;
		Vector3 rhs = base.transform.rotation * Vector3.up;
		if (Vector3.Cross(lhs, rhs).z < 0f)
		{
			num = -num;
		}
		this.m_angularVel.z = 0.0174532924f * num / Time.fixedDeltaTime;
		this.m_prevPos = base.transform.position;
		this.m_prevRot = base.transform.rotation;
	}

	public bool m_animationMode = true;

	private bool m_bNextUpdate;

	private bool m_bUpdate;

	private Vector2 m_prevPos;

	private Quaternion m_prevRot;

	private Vector2 m_nextPos;

	private Quaternion m_nextRot;

	private Vector2 m_linearVel;

	private Vector3 m_angularVel;
}
