using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	private void Start()
	{
		this.lastPosition = base.transform.position;
	}

	private void Update()
	{
		float num = this.trailReference.spline.Length();
		this.distance = Mathf.Clamp(this.distance + this.speed * Time.deltaTime, 0f, num - 0.1f);
		this.trailReference.maxLength = Mathf.Max(num - this.distance, 0f);
		Vector3 vector = this.trailReference.spline.FindTangentFromDistance(this.distance);
		Vector3 rhs = this.trailReference.spline.FindPositionFromDistance(this.distance);
		if (vector != Vector3.zero)
		{
			if (this.lastPosition == rhs)
			{
				this.anim.CrossFade("Idle");
			}
			else
			{
				this.anim.CrossFade("Walk");
			}
			base.transform.forward = vector;
			base.transform.position = (this.lastPosition = rhs);
		}
	}

	public float speed;

	public SplineTrailRenderer trailReference;

	public Animation anim;

	private float distance;

	private Vector3 lastPosition;
}
