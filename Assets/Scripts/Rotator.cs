using System;
using CIS;
using UnityEngine;

public class Rotator : MonoBehaviour, ILevelItem
{
	private void Awake()
	{
		this.myTrans = base.transform;
		this.initQuat = this.myTrans.rotation;
		this.triggered = true;
		if (this.needDoorTrigger || this.needAreaTrigger)
		{
			this.triggered = false;
		}
		this.originalDirection = this.direction;
	}

	public void OnReset()
	{
		if (!this.needReset)
		{
			return;
		}
		this.direction = this.originalDirection;
		this.myTrans.rotation = this.initQuat;
		this.triggered = true;
		if (this.needDoorTrigger || this.needAreaTrigger)
		{
			this.triggered = false;
		}
	}

	public void OnPause(bool isPause)
	{
	}

	private void Update()
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance != null && SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		if (!this.triggered)
		{
			return;
		}
		float z = this.myTrans.rotation.eulerAngles.z;
		this.myTrans.rotation = Quaternion.Euler(0f, 0f, z + (float)this.direction * this.speed * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
	}

	public void OnAreaTriggerEnter(Collider2D coll)
	{
		if (coll.CompareTag("Player"))
		{
			this.triggered = true;
		}
	}

	public void OnAreaTriggerExit(Collider2D coll)
	{
	}

	public void OnActive(int index)
	{
		this.triggered = true;
	}

	public void OnEntertBlock(ILevelBlock block)
	{
	}

	public void OnLeaveBlock(ILevelBlock block)
	{
	}

	public void OnPickingTime(float time)
	{
	}

	public float speed;

	public int direction = 1;

	public bool needReset;

	public bool needAreaTrigger;

	public bool needDoorTrigger;

	private Transform myTrans;

	private Quaternion initQuat;

	private int originalDirection;

	private bool triggered = true;
}
