using System;
using GameWorld;
using UnityEngine;

[RequireComponent(typeof(SpineAnimation))]
public class BattleZoneGate : BaseBehaviour
{
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.box = base.GetComponent<Collider2D>();
		this.box.enabled = false;
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<HitWallArgs>("HitWall", new EventManager.FBEventHandler<HitWallArgs>(this.HitWall), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<HitWallArgs>("HitWall", new EventManager.FBEventHandler<HitWallArgs>(this.HitWall), EventManager.ListenerQueue.Game);
	}

	private bool HitWall(string name, object sender, HitWallArgs msg)
	{
		float x = msg.who.transform.position.x;
		float diameter = 1f;
		bool flag = MathfX.isInMiddleRange(x, base.transform.position.x, diameter);
		if (flag && this.isAppearing)
		{
			this.animator.Play("Hit");
		}
		return true;
	}

	public void Appear()
	{
		this.animator.Play("Open");
		this.isAppearing = true;
	}

	public void DisAppear()
	{
		this.animator.Play("DisAppear");
		this.isAppearing = false;
	}

	public Transform positionCamera;

	public Transform playerLimitPos;

	public BattleZoneGate.GateType type;

	private Animator animator;

	private float lastCameraX;

	private float lastPlayerX;

	private bool isAppearing;

	private Collider2D box;

	public enum GateType
	{
		Left = 1,
		Right,
		None = 0
	}
}
