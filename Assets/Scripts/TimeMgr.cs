using System;
using CIS;
using UnityEngine;

public class TimeMgr : SingletonMonoBehaviourClass<TimeMgr>
{
	public float deltaTime
	{
		get
		{
			return this._deltaTime;
		}
	}

	private void Start()
	{
		this.lastRealTime = Time.realtimeSinceStartup;
		if (this.mode == TimeMgr.TimeCountMode.Fix)
		{
			this._deltaTime = this.fixDeltaTime;
		}
	}

	private void Update()
	{
		if (this.mode == TimeMgr.TimeCountMode.RealTime)
		{
			this.realDeltaTime = Time.realtimeSinceStartup - this.lastRealTime;
			this.lastRealTime = Time.realtimeSinceStartup;
			this.realDeltaTime = Mathf.Clamp(this.realDeltaTime, 0f, 0.05f);
			this._deltaTime = this.realDeltaTime * Time.timeScale;
		}
	}

	private float realDeltaTime;

	private float lastRealTime;

	private float _deltaTime;

	public TimeMgr.TimeCountMode mode;

	public float fixDeltaTime;

	public enum TimeCountMode
	{
		RealTime,
		Fix
	}
}
