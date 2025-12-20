using System;
using CIS;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{
	private void Awake()
	{
		this.myTrans = base.transform;
	}

	private void Update()
	{
		if (this.emmit)
		{
			this.centerTrans.Rotate(0f, 0f, this.speed * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
			this.rotateTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			if (this.rotateTimer >= this.rotateTime)
			{
				this.emmit = false;
				this.rotateTimer = 0f;
				this.centerTrans.rotation = Quaternion.Euler(0f, 0f, this.endDegree);
			}
		}
	}

	public void Emmit(float startDegree, float endDegree, float duration)
	{
		this.emmit = true;
		this.speed = Mathf.Abs(endDegree - startDegree) / duration;
		this.rotateTime = duration;
		this.endDegree = endDegree;
		this.centerTrans.rotation = Quaternion.Euler(0f, 0f, startDegree);
	}

	public Transform centerTrans;

	private Transform myTrans;

	private float radius;

	private bool emmit;

	private float rotateTime;

	private float rotateTimer;

	private float endDegree;

	private float speed;
}
