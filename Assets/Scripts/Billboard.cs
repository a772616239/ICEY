using System;
using UnityEngine;

public class Billboard : BaseBehaviour
{
	private void Awake()
	{
		if (this.AutoInitCamera)
		{
			this.Camera = Camera.main;
			this.Active = true;
		}
		this.camT = this.Camera.transform;
		this.contT = base.transform;
	}

	private void Update()
	{
		if (this.Active)
		{
			this.contT.LookAt(this.contT.position + this.camT.rotation * Vector3.back, this.camT.rotation * Vector3.up);
		}
	}

	public Camera Camera;

	public bool Active = true;

	public bool AutoInitCamera = true;

	private GameObject myContainer;

	private Transform camT;

	private Transform contT;
}
