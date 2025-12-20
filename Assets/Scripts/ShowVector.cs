using System;
using UnityEngine;

public class ShowVector : BaseBehaviour
{
	private void Start()
	{
		this.lastPos = base.transform.position;
	}

	private void Update()
	{
		if (this.type == ShowVector.update.Update)
		{
			this.UpdateVel(Time.smoothDeltaTime);
		}
	}

	private void LateUpdate()
	{
		if (this.type == ShowVector.update.LateUpdate)
		{
			this.UpdateVel(Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		if (this.type == ShowVector.update.FixedUpdate)
		{
			this.UpdateVel(Time.fixedDeltaTime);
		}
	}

	private void UpdateVel(float time)
	{
		this.vec = base.transform.position - this.lastPos;
		this.vec /= time;
		this.lastPos = base.transform.position;
	}

	public Vector3 lastPos;

	public Vector3 vec;

	public ShowVector.update type;

	public enum update
	{
		Update,
		LateUpdate,
		FixedUpdate
	}
}
