using System;
using CIS;
using UnityEngine;

public class SinMove : MonoBehaviour, ILevelItem
{
	private void Awake()
	{
		this.myTrans = base.transform;
		this.initPos = this.myTrans.position;
		this.originalPos = this.initPos;
		if (this.autoRotate)
		{
			this.radius = Vector2.Distance(this.initPos, this.rotateCenterTrans.position);
		}
		this.sinDirectionTimer = 0f;
		this.sinDirectionTime = UnityEngine.Random.Range(2f, 3f);
	}

	public void SetInitPos(Vector2 pos)
	{
		this.initPos = pos;
		if (this.autoRotate)
		{
			this.radius = Vector2.Distance(this.initPos, this.rotateCenterTrans.position);
			Vector2 vector = this.initPos - new Vector2(this.rotateCenterTrans.position.x, this.rotateCenterTrans.position.y);
			this.autoRotateTimer = Mathf.Atan2(vector.y, vector.x);
		}
	}

	public Vector2 GetCurrentPos()
	{
		return new Vector2(this.initPos.x + this.bias * Mathf.Cos(this.timer), this.initPos.y + this.bias * Mathf.Sin(this.timer + 0.3926991f));
	}

	private void Start()
	{
		if (this.randomStart)
		{
			this.waitTime = UnityEngine.Random.Range(0.3f, 4f);
		}
		else
		{
			this.waitTime = 0f;
		}
		this.timer = this.waitTime;
		if (this.autoRotate)
		{
			Vector2 vector = this.myTrans.position - this.rotateCenterTrans.position;
			this.autoRotateTimer = Mathf.Atan2(vector.y, vector.x);
		}
	}

	public void OnReset()
	{
		this.direction = 1;
		this.myTrans.position = this.originalPos;
		this.initPos = this.originalPos;
		this.sinDirectionTimer = 0f;
		this.sinDirection = 1;
	}

	public void OnPause(bool isPause)
	{
	}

	public Vector2 GetRotateLinearVel()
	{
		if (this.autoRotate)
		{
			Vector2 v = this.rotateCenterTrans.position - this.myTrans.position;
			return Quaternion.Euler(0f, 0f, 90f) * v * this.rotateSpeed * (float)(-(float)this.direction);
		}
		return Vector2.zero;
	}

	private void Update()
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		this.timer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.sinDirectionTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		if (this.sinDirectionTimer > this.sinDirectionTime)
		{
			this.sinDirection *= -1;
			this.sinDirectionTimer = 0f;
			this.sinDirectionTime = UnityEngine.Random.Range(2f, 3f);
		}
		if (this.autoRotate)
		{
			this.autoRotateTimer += this.rotateSpeed * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime * (float)this.direction;
			this.initPos.x = this.rotateCenterTrans.position.x + this.radius * Mathf.Cos(this.autoRotateTimer);
			this.initPos.y = this.rotateCenterTrans.position.y + this.radius * Mathf.Sin(this.autoRotateTimer);
		}
		this.myTrans.position = new Vector2(this.initPos.x + this.bias * Mathf.Cos(this.timer), this.initPos.y + this.bias * Mathf.Sin(this.timer + 0.3926991f));
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

	public bool randomStart;

	public float bias;

	public bool autoRotate;

	public Transform rotateCenterTrans;

	public float rotateSpeed;

	public int direction = 1;

	private Transform myTrans;

	private Vector2 initPos;

	private Vector2 originalPos;

	private float waitTime;

	private float timer;

	private float autoRotateTimer;

	private float radius;

	private int sinDirection = 1;

	private float sinDirectionTimer;

	private float sinDirectionTime;
}
