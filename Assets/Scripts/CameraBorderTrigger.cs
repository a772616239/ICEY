using System;
using CIS;
using UnityEngine;

public class CameraBorderTrigger : MonoBehaviour
{
	private void Awake()
	{
		this.myTrans = base.transform;
	}

	private void Start()
	{
		this.cameraTrans = SingletonMonoBehaviourClass<SHICameraController>.instance.transform;
	}

	private void Update()
	{
		if (this.entered)
		{
			return;
		}
		if (this.towards == CameraBorderTrigger.Towards.right && this.cameraTrans.position.x + this.cameraWidth / 2f > this.myTrans.position.x && this.handler != null)
		{
			this.entered = true;
			this.handler._onTriggerEnter2D(PlayerControllerDemo.Instance.GetComponent<Collider2D>());
		}
	}

	private Transform myTrans;

	private float cameraWidth = 1920f;

	private float cameraHight = 1080f;

	private Transform cameraTrans;

	private CameraBorderTrigger.Towards towards;

	public ColliderTriggerHandler handler;

	public bool entered;

	public enum Towards
	{
		right,
		left,
		up,
		down
	}
}
