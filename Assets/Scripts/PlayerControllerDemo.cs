using System;
using System.Collections.Generic;
using CIS;
using DG.Tweening;
using MonsterLove.StateMachine;
using Spine;
using UnityEngine;
using Xft;

public class PlayerControllerDemo : MonsterLove.StateMachine.StateMachineBehaviour, IBlowable, ILevelItem, IInputEventHandler, IPhysicsEventHandler, IPlayerController
{
	public static PlayerControllerDemo Instance
	{
		get
		{
			if (PlayerControllerDemo._instance == null)
			{
				PlayerControllerDemo._instance = UnityEngine.Object.FindObjectOfType<PlayerControllerDemo>();
			}
			return PlayerControllerDemo._instance;
		}
	}

	public PlayerControllerDemo.ActionState currState
	{
		get
		{
			if (this.inited)
			{
				return (PlayerControllerDemo.ActionState)base.GetState();
			}
			return PlayerControllerDemo.ActionState.stand;
		}
	}

	public Vector2 Vel
	{
		get
		{
			return this.vel;
		}
	}

	public Vector2 Accel
	{
		get
		{
			return this.accel;
		}
	}

	public void _OnWindEnter(IBlowee localWind)
	{
		if (this.restting)
		{
			return;
		}
		this.bloweeStack.Add(localWind);
		this.accel = Vector2.zero;
		base.ChangeState(PlayerControllerDemo.ActionState.passiveAirWithWind, null);
	}

	public void _OnWindExit(IBlowee localWind)
	{
		if (this.restting)
		{
			return;
		}
		if (this.bloweeStack.Contains(localWind))
		{
			this.bloweeStack.Remove(localWind);
		}
	}

	public Bounds GetBounds()
	{
		return base.GetComponent<CircleCollider2D>().bounds;
	}

	public void KeyDown(KeyCode key)
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		this.CheckInputDown(key);
	}

	public void KeyUp(KeyCode key)
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		this.CheckInputUp(key);
	}

	public void OnReset()
	{
		if (this.invisible)
		{
			return;
		}
		this.restting = true;
		base.GetComponent<BoxCollider2D>().enabled = false;
		this.deathCount++;
		this.EndCheckAttack();
		this.blockInput = true;
		this.ResetInput();
		base.PauseStateMachine(true);
		this.animator.Play("Fall");
		base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
		this.accel = Vector2.zero;
		this.vel = Vector2.zero;
		this.inputDir = Vector2.zero;
		this.guid = Guid.NewGuid().ToString();
		Skeleton skeleton = base.GetComponent<SkeletonAnimator>().skeleton;
		DOTween.To(delegate(float x)
		{
			skeleton.a = x;
		}, skeleton.a, 0f, this.death2GhostClip.length * 0.5f);
		this.deathAnimator.transform.position = new Vector2(this.myTrans.position.x, this.myTrans.position.y + 75f);
		this.deathAnimator.Play("death2GhostPure");
		this.bloweeStack.Clear();
		Sequence s = DOTween.Sequence();
		s.AppendInterval(this.death2GhostClip.length);
		s.AppendCallback(delegate
		{
			this.deathAnimator.Play("death2Ghost");
			Vector2 b = this.myTrans.position;
			Vector2 vector = this.respawnPoint - b;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this.deathAnimator.transform.rotation = Quaternion.Euler(0f, 0f, z);
			this.deathAnimator.transform.position = new Vector2(this.myTrans.position.x, this.myTrans.position.y + 75f);
			if (this.respawnPoint != Vector2.zero)
			{
				this.myTrans.DOMove(new Vector3(this.respawnPoint.x, this.respawnPoint.y, this.myTrans.position.z), 0.3f, false);
				this.deathAnimator.transform.DOMove(new Vector3(this.respawnPoint.x, this.respawnPoint.y + 75f, this.myTrans.position.z), 0.3f, false);
				this.Invoke("Reset", 0.32f);
			}
			else
			{
				Log.Error("haven't set respawnPoint before use");
			}
		});
	}

	public void OnPause(bool isPause)
	{
		if (isPause)
		{
			this.animator.enabled = false;
			this.freezeTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
		else
		{
			this.animator.enabled = true;
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			this.animator.Play(currentAnimatorStateInfo.fullPathHash, 0, this.freezeTime);
		}
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

	public void _OnTriggerEnter(Collider2D other)
	{
		if (this.restting)
		{
			return;
		}
		if (other.CompareTag("Land"))
		{
			this.hasLand = true;
			if (this.currState == PlayerControllerDemo.ActionState.jump || this.currState == PlayerControllerDemo.ActionState.passiveAir)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.land, null);
			}
		}
		else if (other.CompareTag("Lantern") || other.CompareTag("Attackable"))
		{
			this.isInLantern = true;
			this.currAttackableList.Add(other.gameObject.GetComponent<Attackable>());
			if (this.isAttack && this.checkAttackLantern())
			{
				this.isAttack = false;
				this.attackWindowTimer = 0f;
				this.realAttack = true;
				if (this.currState != PlayerControllerDemo.ActionState.passiveAir)
				{
					base.ChangeState(PlayerControllerDemo.ActionState.attack, null);
				}
			}
		}
		else if (other.CompareTag("Gear") && !this.invisible)
		{
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.Reset();
			PlayerDataMgr.Instance.PersistSectionData();
			HurtableDotsGetter component = other.GetComponent<HurtableDotsGetter>();
			if (component != null)
			{
				ParticleSystemAttractor component2 = base.GetComponent<ParticleSystemAttractor>();
				if (component2 != null)
				{
					component2.Attract(component.dotsParticleSystem, this.myTrans, new Vector3(0f, 100f, 0f), 2.2f);
				}
			}
		}
	}

	public void _OnTriggerExit(Collider2D other)
	{
		if (this.restting)
		{
			return;
		}
		if (other.CompareTag("Lantern") || other.CompareTag("Attackable"))
		{
			Attackable component = other.gameObject.GetComponent<Attackable>();
			for (int i = 0; i < this.currAttackableList.Count; i++)
			{
				if (component == this.currAttackableList[i])
				{
					this.currAttackableList.RemoveAt(i);
					break;
				}
			}
			if (this.currAttackableList.Count == 0)
			{
				this.isInLantern = false;
			}
		}
		else if (other.CompareTag("Land"))
		{
			this.hasLand = false;
			if (this.currState == PlayerControllerDemo.ActionState.jump || this.currState == PlayerControllerDemo.ActionState.passiveAir)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
			}
		}
	}

	public void _OnTriggerStay(Collider2D collider)
	{
	}

	public void _OnCollisionStay(Collider2D collider)
	{
		if (collider.CompareTag("Land") && this.currState == PlayerControllerDemo.ActionState.passiveAir && !this.landBeWall)
		{
			this.hasLand = true;
		}
	}

	public bool _OnCollisionEnter(Collider2D coll, Vector2 normal)
	{
		if (this.restting)
		{
			return false;
		}
		if (coll.CompareTag("Land"))
		{
			SoundMgr.Instance.PlaySfx("PawnDown", 0.7f, false, 0f, 0f);
			if (normal.x > 0f)
			{
				this.isCollideWithWall = true;
				this.landBeWall = true;
				this.currContactNormal = normal;
				return true;
			}
			if (normal.x < 0f)
			{
				this.isCollideWithWall = true;
				this.landBeWall = true;
				this.currContactNormal = normal;
				return true;
			}
			if (normal.y <= 0f)
			{
				this.accel.y = 0f;
				this.vel.y = 0f;
				return false;
			}
			this.hasLand = true;
			PlayerCollisionChecker playerCollisionChecker = this.checkerForLand;
			if (this.myTrans.position.y + playerCollisionChecker.offset.y - playerCollisionChecker.size.y / 2f < coll.bounds.center.y + coll.bounds.extents.y)
			{
				this.myTrans.position = new Vector2(this.myTrans.position.x, coll.bounds.center.y + coll.bounds.extents.y + playerCollisionChecker.size.y / 2f - playerCollisionChecker.offset.y - 1f);
			}
			if (this.currState == PlayerControllerDemo.ActionState.roll || this.currState == PlayerControllerDemo.ActionState.jump || this.currState == PlayerControllerDemo.ActionState.passiveAir || this.currState == PlayerControllerDemo.ActionState.attack || this.currState == PlayerControllerDemo.ActionState.passiveAirWithWind)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.land, null);
			}
		}
		else if (coll.CompareTag("Wall"))
		{
			this.isCollideWithWall = true;
			this.currContactNormal = normal;
			PlayerCollisionChecker playerCollisionChecker2 = this.checkerForWall;
			float num = 0f;
			float num2 = 0f;
			if (playerCollisionChecker2.shape == PlayerCollisionChecker.Shape.box)
			{
				num = playerCollisionChecker2.size.x / 2f;
				num2 = playerCollisionChecker2.size.y / 2f;
			}
			else if (playerCollisionChecker2.shape == PlayerCollisionChecker.Shape.circle)
			{
				num2 = (num = playerCollisionChecker2.radius);
			}
			if (normal.x < -0.1f)
			{
				if (this.myTrans.position.x + playerCollisionChecker2.offset.x + num > coll.bounds.center.x - coll.bounds.extents.x)
				{
					this.myTrans.position = new Vector2(coll.bounds.center.x - coll.bounds.extents.x - playerCollisionChecker2.offset.x - num + 0.5f, this.myTrans.position.y);
				}
			}
			else if (normal.x > 0.1f && this.myTrans.position.x + playerCollisionChecker2.offset.x - num < coll.bounds.center.x + coll.bounds.extents.x)
			{
				this.myTrans.position = new Vector2(coll.bounds.center.x + coll.bounds.extents.x - playerCollisionChecker2.offset.x + num - 0.5f, this.myTrans.position.y);
			}
			if (normal.y < -0.1f)
			{
				if (this.myTrans.position.y + playerCollisionChecker2.offset.y + num2 > coll.bounds.center.y - coll.bounds.extents.y)
				{
					this.myTrans.position = new Vector2(this.myTrans.position.x, coll.bounds.center.y - coll.bounds.extents.y - playerCollisionChecker2.offset.y - num2);
				}
			}
			else if (normal.y > 0.1f && this.myTrans.position.y + playerCollisionChecker2.offset.y - num2 < coll.bounds.center.y + coll.bounds.extents.y)
			{
				this.myTrans.position = new Vector2(this.myTrans.position.x, coll.bounds.center.y + coll.bounds.extents.y - playerCollisionChecker2.offset.y + num2);
			}
		}
		return true;
	}

	public void _OnCollisionExit(Collider2D coll)
	{
		if (this.restting)
		{
			return;
		}
		if (coll.CompareTag("Land"))
		{
			this.hasLand = false;
			if (this.landBeWall)
			{
				this.landBeWall = false;
				this.isCollideWithWall = false;
			}
		}
		else if (coll.CompareTag("Wall"))
		{
			this.isCollideWithWall = false;
		}
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public void SetRespawnPoint(Vector2 point)
	{
		this.respawnPoint = point;
	}

	public Vector2 Velocity
	{
		get
		{
			if (this.currState == PlayerControllerDemo.ActionState.stand)
			{
				return Vector2.zero;
			}
			return this.Vel;
		}
	}

	public void PlayAnimation(string animeName)
	{
		this.animator.Play(animeName);
	}

	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	private static void log(string str)
	{
		PlayerControllerDemo.pDocument = PlayerControllerDemo.pDocument + "\n" + str;
	}

	public void Stop()
	{
		this.inputDir = Vector2.zero;
		base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
	}

	public void SetDirection(int dir)
	{
		if (dir > 0)
		{
			this.myTrans.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			this.myTrans.localScale = new Vector3(-1f, 1f, 1f);
		}
		this.direction = dir;
	}

	public void ResetInputStatus()
	{
		this.inputDir = Vector2.zero;
	}

	private void Awake()
	{
		this.myTrans = base.transform;
		this.animator = base.GetComponent<Animator>();
		base.Initialize<PlayerControllerDemo.ActionState>();
	}

	private void Start()
	{
		base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
		this.direction = 1;
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			SingletonMonoBehaviourClass<InputMgr>.instance.RegisterInputEventHandler(this);
		}
		this.guid = Guid.NewGuid().ToString();
		PlayerCollisionChecker[] componentsInChildren = this.myTrans.GetComponentsInChildren<PlayerCollisionChecker>();
		foreach (PlayerCollisionChecker playerCollisionChecker in componentsInChildren)
		{
			if (playerCollisionChecker.gameObject.name.Contains("wall"))
			{
				this.checkerForWall = playerCollisionChecker;
			}
			else if (playerCollisionChecker.gameObject.name.Contains("land"))
			{
				this.checkerForLand = playerCollisionChecker;
			}
			playerCollisionChecker.RegisterHandler(this);
		}
		this.inited = true;
		RuntimeAnimatorController runtimeAnimatorController = this.animator.runtimeAnimatorController;
		for (int j = 0; j < runtimeAnimatorController.animationClips.Length; j++)
		{
			if (runtimeAnimatorController.animationClips[j].name == "atk1")
			{
				this.atkAnimationLength = runtimeAnimatorController.animationClips[j].length;
			}
		}
	}

	public void FixSide()
	{
		if ((float)this.direction * this.myTrans.localScale.x < 0f)
		{
			this.myTrans.localScale = new Vector2((float)this.direction, 1f);
		}
	}

	private void CheckInputDown(KeyCode key)
	{
		if (this.blockInput)
		{
			return;
		}
		bool flag = false;
		if (key == KeyCode.R)
		{
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.Reset();
		}
		if (key == KeyCode.T)
		{
			this.deathCount = 0;
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.ClearLevelTimeText();
		}
		if (key == KeyCode.A)
		{
			this.leftButtonPressed = true;
			this.inputDir.x = -1f;
			if (this.direction > 0)
			{
				this.OnDirectionChanged(-1);
				flag = true;
			}
			this.direction = -1;
			if ((this.currState == PlayerControllerDemo.ActionState.run || this.currState == PlayerControllerDemo.ActionState.accRun) && flag)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, "directPlay");
			}
			if (this.currState == PlayerControllerDemo.ActionState.slide && Mathf.Abs(this.vel.x) > this.maxRunSpeed * 0.5f)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, null);
			}
		}
		else if (key == KeyCode.D)
		{
			this.rightButtonPressed = true;
			this.inputDir.x = 1f;
			if (this.direction < 0)
			{
				this.OnDirectionChanged(1);
				flag = true;
			}
			this.direction = 1;
			if ((this.currState == PlayerControllerDemo.ActionState.run || this.currState == PlayerControllerDemo.ActionState.accRun) && flag)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, "directPlay");
			}
			if (this.currState == PlayerControllerDemo.ActionState.slide && Mathf.Abs(this.vel.x) > this.maxRunSpeed * 0.5f)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, null);
			}
		}
		else if (key == KeyCode.J)
		{
			this.actionButtonPressed = true;
		}
	}

	private void CheckInputUp(KeyCode key)
	{
		if (this.blockInput)
		{
			return;
		}
		if (key == KeyCode.A)
		{
			this.leftButtonPressed = false;
		}
		else if (key == KeyCode.D)
		{
			this.rightButtonPressed = false;
		}
		if (key == KeyCode.A && this.inputDir.x == -1f)
		{
			this.inputDir.x = 0f;
		}
		else if (key == KeyCode.D && this.inputDir.x == 1f)
		{
			this.inputDir.x = 0f;
		}
		if (key == KeyCode.J)
		{
			this.actionButtonPressed = false;
		}
	}

	private void CheckInput()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.T))
		{
			this.deathCount = 0;
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.ClearLevelTimeText();
		}
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			return;
		}
		if (this.blockInput)
		{
			return;
		}
		bool flag = false;
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			this.inputDir.x = -1f;
			if (this.direction > 0)
			{
				this.OnDirectionChanged(-1);
				flag = true;
			}
			this.direction = -1;
			PlayerControllerDemo.ActionState currState = this.currState;
			if (currState == PlayerControllerDemo.ActionState.run && flag)
			{
				this.accel = this.Run2StandAccel;
				UnityEngine.Debug.Log("a vel: " + this.vel);
				UnityEngine.Debug.Log("accel: " + this.accel);
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, null);
			}
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.D))
		{
			this.inputDir.x = 1f;
			if (this.direction < 0)
			{
				this.OnDirectionChanged(1);
				flag = true;
			}
			this.direction = 1;
			if (this.currState == PlayerControllerDemo.ActionState.run && flag)
			{
				this.accel = this.Run2StandAccel;
				UnityEngine.Debug.Log("d vel: " + this.vel);
				UnityEngine.Debug.Log("accel: " + this.accel);
				base.ChangeState(PlayerControllerDemo.ActionState.slideTurn, null);
			}
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.A) && this.inputDir.x == -1f)
		{
			this.inputDir.x = 0f;
		}
		else if (UnityEngine.Input.GetKeyUp(KeyCode.D) && this.inputDir.x == 1f)
		{
			this.inputDir.x = 0f;
		}
	}

	private void ApplayGravity(float dt)
	{
		if (this.currAttackableList.Count == 0)
		{
			this.vel.y = this.vel.y + this.gravity.y * dt;
		}
		else
		{
			Attackable nearestAttackable = this.GetNearestAttackable();
			if (nearestAttackable.AffectGravity())
			{
				this.vel.y = this.vel.y + this.gravity.y * nearestAttackable.gravityFactor * dt;
			}
			else
			{
				this.vel.y = this.vel.y + this.gravity.y * dt;
			}
		}
	}

	private void InterpolatePos(float dt)
	{
		if (this.testing)
		{
			return;
		}
		this.tempVec2 = this.myTrans.position;
		if (this.vel.x > this.maxRunSpeed)
		{
			if (this.currState == PlayerControllerDemo.ActionState.accRun)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.run, null);
			}
			this.vel.x = this.maxRunSpeed;
		}
		else if (this.vel.x < -this.maxRunSpeed)
		{
			if (this.currState == PlayerControllerDemo.ActionState.accRun)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.run, null);
			}
			this.vel.x = -this.maxRunSpeed;
		}
		this.vel += this.accel * dt;
		if (this.currState == PlayerControllerDemo.ActionState.passiveAirWithWind)
		{
			if (this.bloweeStack.Count > 0)
			{
				IBlowee blowee = this.bloweeStack[this.bloweeStack.Count - 1];
				LocalWind localWind = blowee as LocalWind;
				if (localWind != null)
				{
					this.vel.y = Mathf.Clamp(this.vel.y, localWind.minHeroYSpeed, localWind.maxHeroYSpeed);
				}
			}
		}
		else
		{
			this.vel.y = Mathf.Clamp(this.vel.y, this.minYVel, this.maxYVel);
		}
		if (this.isCollideWithWall)
		{
			Vector2 rhs = this.currContactNormal;
			float num = Vector2.Dot(this.vel.normalized, rhs);
			if (num < 0f)
			{
				float num2 = Mathf.Atan2(rhs.y, rhs.x) * 57.29578f;
				if ((num2 > -35f && num2 < 35f) || (num2 > 145f && num2 < 215f) || (num2 < -145f && num2 > -215f))
				{
					this.vel.x = 0f;
				}
				if (num2 < -55f && num2 > -125f)
				{
					this.vel.y = 0f;
				}
			}
		}
		if (this.hasLand && this.vel.y < 0f)
		{
			this.vel.y = 0f;
		}
		this.animator.SetFloat("speedX", Mathf.Abs(this.vel.x));
		this.tempVec2.x = this.tempVec2.x + this.vel.x * dt;
		if (!this.hasLand || this.vel.y >= 0f)
		{
			this.tempVec2.y = this.tempVec2.y + this.vel.y * dt;
		}
		this.myTrans.position = this.tempVec2;
		if (this.turnDirection)
		{
			this.turnDirection = false;
		}
	}

	private Vector2 GetInputDir()
	{
		return this.inputDir;
	}

	private void Stand_KeyDown(KeyCode key)
	{
	}

	private void Stand_KeyUp(KeyCode key)
	{
	}

	private void stand_Enter(object userData = null)
	{
		if (this.inputDir.x != 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.run, userData);
			return;
		}
		this.vel = Vector2.zero;
		this.accel = Vector2.zero;
		if (userData != null)
		{
			string text = userData as string;
			if (text != null && text == "directPlay")
			{
				this.animator.Play("StandRun");
			}
		}
		else
		{
			this.animator.SetTrigger("standRun");
		}
	}

	private void stand_Exit(object userData = null)
	{
		this.animator.ResetTrigger("standRun");
	}

	private void stand_Update()
	{
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.CheckInput();
		if (this.inputDir.x > 0f || this.inputDir.x < 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.run, null);
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
		bool flag = this.actionButtonPressed;
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		if (flag && currentAnimatorStateInfo.IsName("StandRun"))
		{
			base.ChangeState(PlayerControllerDemo.ActionState.jump, "preJump");
			return;
		}
		this.InterpolatePos(deltaTime);
	}

	private void slide_Enter(object userData = null)
	{
		this.animator.SetTrigger("slide");
	}

	private void slide_Exit(object userData = null)
	{
		this.animator.ResetTrigger("slide");
	}

	private void slide_Update()
	{
		this.CheckInput();
		if (this.actionButtonPressed)
		{
			this.accel.x = 0f;
			this.actionButtonPressed = false;
			base.ChangeState(PlayerControllerDemo.ActionState.jump, null);
			return;
		}
		bool flag = this.animator.IsInTransition(0);
		if (this.vel.x > 0f)
		{
			this.accel.x = this.Run2StandAccel.x;
			this.slideSign = -1;
		}
		else
		{
			this.accel.x = -this.Run2StandAccel.x;
			this.slideSign = 1;
		}
		if (this.vel.x == 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
			return;
		}
		this.InterpolatePos(SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
		if (!flag)
		{
			if (this.slideSign > 0)
			{
				if (this.vel.x >= 0f)
				{
					if (this.inputDir.x != 0f)
					{
						base.ChangeState(PlayerControllerDemo.ActionState.run, null);
						return;
					}
					base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
					return;
				}
			}
			else if (this.vel.x <= 0f)
			{
				if (this.inputDir.x != 0f)
				{
					base.ChangeState(PlayerControllerDemo.ActionState.run, null);
					return;
				}
				base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
				return;
			}
		}
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
		}
	}

	private void accRun_Enter(object userData = null)
	{
		this.animator.Play("accRun");
		if (this.direction > 0)
		{
			this.accel = this.stand2RunAccel;
		}
		else
		{
			this.accel = this.stand2RunAccel;
			this.accel.x = this.accel.x * -1f;
		}
	}

	private void AnimeEnd_accRun()
	{
		if (this.inputDir.x != 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.run, null);
		}
		else
		{
			base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
		}
	}

	private void accRun_Update()
	{
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		bool flag = this.actionButtonPressed;
		if (flag)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.jump, "preJump");
			return;
		}
		if (this.inputDir.x == 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.slide, null);
			return;
		}
		this.InterpolatePos(SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
		this.PlayFootstepSound();
	}

	private void slideTurn_Enter(object userData = null)
	{
		if (userData != null)
		{
			this.animator.Play("SlideTurn");
		}
		else
		{
			this.animator.SetTrigger("slideTurn");
		}
		this.PlayTurnSmokeAnimation(1f);
	}

	private void slideTurn_Exit(object userData = null)
	{
		this.animator.ResetTrigger("slideTurn");
	}

	private void AnimeEnd_slideTurn()
	{
		if (this.inputDir.x != 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.accRun, null);
		}
		else
		{
			base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
		}
	}

	private void slideTurn_Update()
	{
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		this.CheckInput();
		if (this.vel.x > 0f)
		{
			this.accel.x = this.Run2StandAccel.x;
			this.slideSign = -1;
		}
		else
		{
			this.accel.x = -this.Run2StandAccel.x;
			this.slideSign = 1;
		}
		if (this.vel.x == 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
			return;
		}
		this.InterpolatePos(SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
	}

	private void OnDirectionChanged(int dir)
	{
		if (dir > 0)
		{
			this.myTrans.localScale = new Vector3(1f, 1f, 1f);
			this.accel.x = this.stand2RunAccel.x;
		}
		else
		{
			this.myTrans.localScale = new Vector3(-1f, 1f, 1f);
			this.accel.x = -this.stand2RunAccel.x;
		}
		if (this.currentEffect != null)
		{
			this.currentEffect.transform.localScale = ((this.currentEffect.transform.lossyScale.x <= 0f) ? new Vector2(-1f, 1f) : new Vector2(1f, 1f));
			this.currentEffect.SetScale(this.myTrans.localScale, "slash_distortion");
		}
		this.turnDirection = true;
	}

	private void ResetFootstepSound()
	{
		this.audioTimer = 0f;
	}

	private void PlayFootstepSound()
	{
		this.audioTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		if (this.audioTimer > 0.27f)
		{
			this.audioTimer = 0f;
			SoundMgr.Instance.PlaySfx("FootStep", 1f, false, 0f, 0f);
		}
	}

	private void run_Enter(object userData = null)
	{
		if (this.direction > 0)
		{
			this.accel = this.stand2RunAccel;
		}
		else
		{
			this.accel = this.stand2RunAccel;
			this.accel.x = this.accel.x * -1f;
		}
		if (userData != null)
		{
			string text = userData as string;
			if (text != null && text == "directPlay")
			{
				this.animator.Play("StandRun");
			}
		}
		else
		{
			this.animator.SetTrigger("standRun");
		}
		this.ResetFootstepSound();
	}

	private void run_Exit(object userData = null)
	{
		this.ResetFootstepSound();
		this.animator.ResetTrigger("standRun");
	}

	private void run_Update()
	{
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.CheckInput();
		if (Mathf.Abs(this.vel.x) > this.maxRunSpeed)
		{
			this.accel.x = 0f;
		}
		if (this.inputDir.x == 0f)
		{
			if (this.vel.x != 0f)
			{
				this.accel = this.Run2StandAccel;
				base.ChangeState(PlayerControllerDemo.ActionState.slide, null);
			}
			else
			{
				base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
			}
			return;
		}
		bool keyDown;
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			keyDown = this.actionButtonPressed;
		}
		else
		{
			keyDown = UnityEngine.Input.GetKeyDown(KeyCode.J);
		}
		if (keyDown)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.jump, "preJump");
			return;
		}
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		this.InterpolatePos(deltaTime);
		this.PlayFootstepSound();
	}

	private void PlayLandSmokeAnimation(float speed)
	{
		this.smokeAnimator.transform.parent = this.myTrans.parent;
		this.smokeAnimator.transform.position = this.smokeTargetTrans.position;
		this.smokeAnimator.speed = speed;
		this.smokeAnimator.Play("smoke");
	}

	private void PlayTurnSmokeAnimation(float speed)
	{
		this.smokeTurnAnimator.transform.parent = this.myTrans.parent;
		this.smokeTurnAnimator.transform.position = this.smokeTurnTargetTrans.position;
		this.smokeTurnAnimator.transform.localScale = new Vector2((float)(-(float)this.direction), 1f);
		this.smokeTurnAnimator.speed = speed;
		if (this.smokeTurnAnimator.GetCurrentAnimatorStateInfo(0).IsName("smoke"))
		{
			this.smokeTurnAnimator.Play("smoke1");
		}
		else if (this.smokeTurnAnimator.GetCurrentAnimatorStateInfo(0).IsName("smoke1"))
		{
			this.smokeTurnAnimator.Play("smoke");
		}
		else
		{
			this.smokeTurnAnimator.Play("smoke");
		}
	}

	private void preJump_Enter(object userData = null)
	{
		this.animator.SetTrigger("jump");
		this.PlayLandSmokeAnimation(2f);
		this.actionButtonPressed = false;
		SoundMgr.Instance.PlaySfx("PawnJump", 0.7f, false, 0f, 0f);
	}

	private void preJump_Update()
	{
		this.CheckInput();
		this.InterpolatePos(SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime);
	}

	private void AnimeEnd_preJump()
	{
		base.ChangeState(PlayerControllerDemo.ActionState.jump, null);
	}

	private void jump_Enter(object userData)
	{
		if (userData is string)
		{
			string a = (string)userData;
			if (a == "preJump")
			{
				this.animator.Play("preJump");
			}
		}
		else
		{
			this.animator.SetTrigger("jump");
		}
		this.PlayLandSmokeAnimation(2f);
		this.actionButtonPressed = false;
		SoundMgr.Instance.PlaySfx("PawnJump", 0.7f, false, 0f, 0f);
		if (this.bloweeStack.Count > 0)
		{
			IBlowee blowee = this.bloweeStack[this.bloweeStack.Count - 1];
			this.vel.y = this.initJumpSpeed.y;
			this.accel = this.JumpAccel;
		}
		else
		{
			this.vel.y = this.initJumpSpeed.y;
			this.accel = this.JumpAccel;
		}
		this.jumpAccelTimer = 0f;
		this.isFall = false;
	}

	private void jump_Exit(object userData = null)
	{
		this.animator.ResetTrigger("jump");
	}

	private Tween Impulse(Vector2 _accel, Vector2 _vel, float _t, string animationName = null)
	{
		string oldGUID = this.guid;
		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(_t);
		sequence.OnComplete(delegate
		{
			if (this.guid == oldGUID)
			{
				this.vel.y = _vel.y;
				this.accel = _accel;
				if (this.bloweeStack.Count > 0)
				{
					this.accel = Vector2.one * 0.001f;
				}
				this.controlPointTrans.position = this.myTrans.position;
				this.tempT1 = Time.time;
				if (animationName != null)
				{
					this.animator.Play(animationName);
					if (animationName == "AfterHit")
					{
						this.animator.ResetTrigger("attack");
					}
				}
			}
		});
		return sequence;
	}

	private float _length(float v0, float a, float t)
	{
		return v0 * t + 0.5f * a * Mathf.Pow(t, 2f);
	}

	private bool CalculateAttackLantern(Attackable currAttackable)
	{
		Attackable[] array = currAttackable.Next();
		if (!currAttackable.Available())
		{
			return false;
		}
		bool result = false;
		foreach (Attackable attackable in array)
		{
			if (attackable != null)
			{
				Vector2 a = attackable.transform.position;
				Vector2 vector = this.myTrans.position;
				Vector2 zero = Vector2.zero;
				float futureTime = currAttackable.GetFutureTime(attackable);
				float num = this.gravity.y * currAttackable.gravityFactor;
				float num2 = 0f;
				float num3 = 0f;
				float num4;
				if (Mathf.Abs(this.vel.x) < Mathf.Abs(this.maxRunSpeed))
				{
					num4 = (Mathf.Abs(this.maxRunSpeed) - Mathf.Abs(this.vel.x)) / this.stand2RunAccel.x;
					if (num4 > futureTime)
					{
						num4 = futureTime;
					}
				}
				else
				{
					num4 = futureTime;
				}
				float num5;
				if (num4 < futureTime)
				{
					num5 = this._length(this.vel.x, this.accel.x, num4);
					num2 = futureTime - num4;
				}
				else
				{
					num5 = this._length(this.vel.x, this.accel.x, futureTime);
				}
				float num6 = num4 + num2;
				zero.x = vector.x + num5 + num3;
				if (this.vel.y > 0f)
				{
					float num7 = -this.vel.y / num;
					if (num7 > num6)
					{
						num7 = num6;
						float num8 = this._length(this.vel.y, num, num7);
						zero.y = vector.y + num8;
					}
					else
					{
						float num9 = this._length(this.vel.y, num, num7);
						float num10 = this._length(0f, num, num6 - num7);
						zero.y = vector.y + num9 + num10;
					}
				}
				else
				{
					zero.y = vector.y + this._length(this.vel.y, num, num6);
				}
				this.controlPointTrans.position = zero;
				Vector2 vector2 = a - zero;
				if ((float)this.direction * vector2.x < 0f)
				{
					Vector2 b = currAttackable.transform.position;
					Vector2 vector3 = a - b;
					a.x = b.x - vector3.x;
					vector2 = a - zero;
				}
				float num11 = (this.maxRunSpeed - Mathf.Abs(this.vel.x)) / this.stand2RunAccel.x;
				if (num11 < 0f)
				{
					num11 = 0f;
				}
				float num12 = this._length(this.vel.x, this.stand2RunAccel.x, num11);
				float num13 = Mathf.Abs(vector2.x) - num12;
				float num14 = num13 / this.maxRunSpeed;
				if (num14 < 0f)
				{
					num14 = 0f;
				}
				float num15 = num11 + num14;
				float num16 = Mathf.Abs(vector2.y);
				float num17 = Mathf.Abs(Mathf.Sqrt(num16 * 2f / (3f * -num)));
				float num18 = Mathf.Max(num15, num17);
				float num19;
				if (num17 > num15)
				{
					num19 = Mathf.Sqrt(Mathf.Abs(2f * num * vector2.y));
				}
				else
				{
					num19 = Mathf.Abs(vector2.y) / num18 - num * num18 * 0.5f;
				}
				float num20 = this._length(num19, num, num18);
				this.controlPointTrans.position = new Vector2(zero.x + num12 + num13, zero.y + num20);
				Vector2 zero2 = this.stand2RunAccel;
				if (vector2.x < 0f)
				{
					zero2.x *= -1f;
				}
				zero2 = Vector2.zero;
				if (!currAttackable.Action(attackable))
				{
					return false;
				}
				currAttackable.PlayAnimationAfterTime("lanternHit", futureTime);
				currAttackable.React(this.vel, futureTime);
				SoundMgr.Instance.PlaySfx("HitPaperLantern", 0.8f, false, futureTime, 0f);
				if (currAttackable.IsFixYSpeed(attackable))
				{
					Tween jumpTween = this.Impulse(zero2, new Vector2(0f, currAttackable.overrideVelY), futureTime, null);
					currAttackable.SetJumpTween(jumpTween);
				}
				else
				{
					Tween jumpTween2 = this.Impulse(zero2, new Vector2(0f, num19), futureTime, "AfterHit");
					currAttackable.SetJumpTween(jumpTween2);
				}
				result = true;
			}
		}
		return result;
	}

	private Attackable GetNearestAttackable()
	{
		if (this.currAttackableList.Count == 0)
		{
			return null;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 0; i < this.currAttackableList.Count; i++)
		{
			float num2 = Vector2.Distance(this.currAttackableList[i].transform.position, this.myTrans.position);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		return this.currAttackableList[index];
	}

	private bool checkAttackLantern()
	{
		if (this.isInLantern && !this.hasLand)
		{
			Attackable nearestAttackable = this.GetNearestAttackable();
			return this.CalculateAttackLantern(nearestAttackable);
		}
		return false;
	}

	private void PlayAttackAnime()
	{
	}

	private bool CanPlayAttackAnime(AnimatorStateInfo currState, Attackable currAttackable)
	{
		return currState.IsName("Fly2fall") || currState.IsName("Fall") || currState.IsTag("Fly") || currState.IsTag("InWind");
	}

	private void CheckAttack()
	{
		if (this.hasLand)
		{
			return;
		}
		bool keyDown;
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			keyDown = this.actionButtonPressed;
		}
		else
		{
			keyDown = UnityEngine.Input.GetKeyDown(KeyCode.J);
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
		Attackable nearestAttackable = this.GetNearestAttackable();
		if (keyDown && this.CanPlayAttackAnime(currentAnimatorStateInfo, nearestAttackable))
		{
			int value = UnityEngine.Random.Range(1, 2);
			this.animator.SetTrigger("attack");
			this.animator.SetInteger("atkIdx", value);
			if (this.atkAnimator.GetCurrentAnimatorStateInfo(0).IsName("atkFire1"))
			{
				this.atkAnimator.Play("atkFire2");
			}
			else
			{
				this.atkAnimator.Play("atkFire1");
			}
			base.Invoke("PlayAttackAnime", this.attackFireDelay);
			this.attackWindowTime = this.atkAnimationLength / 2f;
			this.attackWindowTimer = 0f;
			this.isAttack = true;
			this.isFall = false;
			this.realAttack = false;
			this.actionButtonPressed = false;
			XffectComponent eft = this.effectPool.GetEffect("AtkTrail");
			if (eft != null)
			{
				eft.gameObject.SetActive(true);
				this.currentEffect = eft;
				eft.GetComponent<XffectHelper>().SetVortexSpeed(3000f);
				eft.transform.position = this.rotateRootTrans.position;
				eft.transform.parent = this.rotateRootTrans;
				if (this.rotateRootTrans.lossyScale.x < 0f)
				{
					eft.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				else
				{
					eft.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				eft.GetComponent<XffectHelper>().SetDir(this.direction);
				eft.Active();
				Sequence s = DOTween.Sequence();
				s.AppendInterval(0.21f);
				s.AppendCallback(delegate
				{
					eft.GetComponent<XffectHelper>().SetVortexSpeed(0f);
					eft.StopSmoothly(0.5f);
				});
				s.AppendInterval(0.5f);
				s.AppendCallback(delegate
				{
					if (this.currentEffect != null)
					{
						this.currentEffect.transform.parent = this.effectPool.transform;
						this.currentEffect = null;
					}
				});
			}
		}
		if (this.isAttack)
		{
			this.attackWindowTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			if (this.attackWindowTimer < this.attackWindowTime)
			{
				if (this.checkAttackLantern())
				{
					this.isAttack = false;
					this.attackWindowTimer = 0f;
					this.realAttack = true;
					if ((PlayerControllerDemo.ActionState)base.GetState() != PlayerControllerDemo.ActionState.passiveAir)
					{
						base.ChangeState(PlayerControllerDemo.ActionState.attack, null);
					}
				}
			}
			else
			{
				this.isAttack = false;
				this.attackWindowTimer = 0f;
			}
		}
	}

	public void EndCheckAttack()
	{
		this.isAttack = false;
		this.attackWindowTimer = 0f;
		this.animator.speed = 1f;
		if (this.currentEffect != null)
		{
			this.currentEffect.StopSmoothly(0.5f);
			this.currentEffect.transform.parent = this.effectPool.transform;
		}
	}

	private void jump_Update()
	{
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.CheckInput();
		this.CheckAttack();
		this.DampingVel(deltaTime);
		this.ApplayGravity(deltaTime);
		this.InterpolatePos(deltaTime);
		if (this.vel.y < this.rollYAxisLimit && this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Fly"))
		{
			this.isFall = true;
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, "roll");
		}
	}

	private void attack_Enter(object userData = null)
	{
		base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, "keepAnime");
	}

	private void attack_Update()
	{
		this.CheckInput();
		this.accel.x = this.inputDir.x * this.stand2RunAccel.x;
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.ApplayGravity(deltaTime);
		this.InterpolatePos(deltaTime);
	}

	private void attack_Exit(object userData = null)
	{
	}

	private void roll_Enter(object userData = null)
	{
		this.animator.SetTrigger("roll");
	}

	private void roll_Update()
	{
		this.CheckInput();
		this.CheckAttack();
		this.accel.x = this.inputDir.x * this.stand2RunAccel.x;
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.ApplayGravity(deltaTime);
		this.InterpolatePos(deltaTime);
	}

	private void roll_Exit(object userData = null)
	{
		this.animator.ResetTrigger("roll");
	}

	private void passiveAir_Enter(object userData = null)
	{
		this.isFall = false;
		if (userData != null && userData.GetType() == typeof(string))
		{
			string text = (string)userData;
			if (text != "keepAnime")
			{
				this.animator.SetTrigger(text);
			}
		}
		else if (this.vel.y <= 0f)
		{
			this.animator.SetTrigger("fall");
		}
		else
		{
			this.animator.SetTrigger("jump");
		}
	}

	private void passiveAir_Exit(object userData = null)
	{
		this.animator.ResetTrigger("fall");
	}

	private bool TestCollideWithLand(Vector2 oldPos, Vector2 newPos)
	{
		if (!this.hasLand && this.vel.y < 0f)
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(this.tempVec2, -Vector2.up);
			Bounds bounds = default(Bounds);
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.CompareTag("Land"))
				{
					flag = true;
					bounds = array[i].collider.bounds;
					break;
				}
			}
			if (flag && newPos.y < bounds.center.y + bounds.extents.y)
			{
				this.vel.y = (bounds.center.y + bounds.extents.y - oldPos.y) / SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime - 1f;
				this.accel.y = 0f;
				return true;
			}
		}
		return false;
	}

	private void DampingVel(float dt)
	{
		if (this.inputDir.x != 0f)
		{
			this.accel.x = this.inputDir.x * this.stand2RunAccel.x;
		}
		else if (this.vel.x * (this.vel.x + this.accel.x * dt) <= 0f)
		{
			this.accel.x = 0f;
			this.vel.x = 0f;
		}
		else
		{
			this.accel.x = (float)((this.vel.x <= 0f) ? -1 : 1) * this.Run2StandAccel.x * this.jumpDampingFactor;
		}
	}

	private void passiveAir_Update()
	{
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		this.CheckInput();
		this.CheckAttack();
		this.DampingVel(deltaTime);
		this.ApplayGravity(deltaTime);
		if (this.accel.x != 0f)
		{
			Attackable nearestAttackable = this.GetNearestAttackable();
			if (nearestAttackable != null)
			{
				this.accel.x = this.accel.x / Mathf.Abs(this.accel.x) * this.stand2RunAccel.x * nearestAttackable.horizenalAccelFactor;
			}
		}
		this.InterpolatePos(deltaTime);
		if (this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Fly") && this.vel.y < this.rollYAxisLimit)
		{
			this.animator.SetTrigger("roll");
		}
		if (this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.land, null);
		}
	}

	private void passiveAirWithWind_Enter(object userData = null)
	{
		this.isFall = false;
		this.animator.SetTrigger("inWind");
		this.hasLand = false;
		if (this.bloweeStack.Count > 0)
		{
			IBlowee blowee = this.bloweeStack[this.bloweeStack.Count - 1];
			Type type = blowee.GetType();
			if (type == typeof(LocalDirectionalWind))
			{
				this.initWindAccel = this.gravity;
			}
		}
	}

	private void passiveAirWithWind_Update()
	{
		if (this.bloweeStack.Count == 0)
		{
			this.accel = Vector2.zero;
			this.windTimer = 0f;
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		this.CheckInput();
		this.CheckAttack();
		this.animator.SetFloat("speedY", this.vel.y);
		float deltaTime = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		if (this.bloweeStack.Count > 0)
		{
			IBlowee blowee = this.bloweeStack[this.bloweeStack.Count - 1];
			Type type = blowee.GetType();
			if (type == typeof(LocalDirectionalWind))
			{
				Vector2 pos = this.myTrans.position;
				if (this.inputDir.x < 0f)
				{
					this.initWindAccel = this.gravity + -this.stand2RunAccel;
				}
				else if (this.inputDir.x > 0f)
				{
					this.initWindAccel = this.gravity + this.stand2RunAccel;
				}
				else
				{
					this.initWindAccel = this.gravity;
				}
				this.accel = this.initWindAccel + blowee.GetDir(this, pos) * blowee.GetPower(this, pos);
				this.InterpolatePos(deltaTime);
			}
		}
		if (this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.land, null);
		}
	}

	private void passiveAirWithWind_Exit(object userData = null)
	{
		this.animator.ResetTrigger("inWind");
	}

	private void Reset()
	{
		base.PauseStateMachine(false);
		this.deathAnimator.Play("ghost2Man");
		this.isCollideWithWall = false;
		this.restting = false;
		this.hasLand = false;
		this.blockInput = false;
		this.isInLantern = false;
		this.currAttackableList.Clear();
		base.GetComponent<BoxCollider2D>().enabled = true;
	}

	private void ResetInput()
	{
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			this.leftButtonPressed = false;
			this.rightButtonPressed = false;
			this.actionButtonPressed = false;
		}
	}

	public void ResetToCheckLastCheckPoint(float time)
	{
		this.restting = true;
		this.blockInput = true;
		this.ResetInput();
		this.currAttackableList.Clear();
		base.PauseStateMachine(true);
		this.animator.Play("Fall");
		base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
		this.accel = Vector2.zero;
		this.vel = Vector2.zero;
		this.inputDir = Vector2.zero;
		this.guid = Guid.NewGuid().ToString();
		if (this.respawnPoint != Vector2.zero)
		{
			this.myTrans.DOMove(new Vector3(this.respawnPoint.x, this.respawnPoint.y, this.myTrans.position.z), time, false);
			base.Invoke("Reset", time);
		}
		else
		{
			Log.Error("haven't set respawnPoint before use");
		}
	}

	private void AnimeEnd_roll()
	{
		this.animator.ResetTrigger("roll");
	}

	private void land_Enter(object userData = null)
	{
		if (this.bloweeStack.Count > 0)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAirWithWind, null);
			return;
		}
		this.PlayLandSmokeAnimation(1f);
		this.animator.SetTrigger("land");
		if (this.inputDir.x != 0f)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.accRun, null);
		}
	}

	private void land_Exit(object userData = null)
	{
		this.animator.ResetTrigger("land");
	}

	private void land_Update()
	{
		this.CheckInput();
		AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
		if (!currentAnimatorStateInfo.IsName("Land"))
		{
			this.animator.SetTrigger("land");
		}
		bool keyDown;
		if (this.inputMethod == PlayerControllerDemo.InputMethod.newMethod)
		{
			keyDown = this.actionButtonPressed;
		}
		else
		{
			keyDown = UnityEngine.Input.GetKeyDown(KeyCode.J);
		}
		if (keyDown && currentAnimatorStateInfo.IsName("Land"))
		{
			UnityEngine.Debug.Log("Land to jump");
			base.ChangeState(PlayerControllerDemo.ActionState.jump, null);
			return;
		}
		if (this.inputDir.x != 0f)
		{
		}
	}

	public void AnimeEnd_atk()
	{
		this.animator.ResetTrigger("attack");
		this.EndCheckAttack();
		if (this.hasLand)
		{
			if (this.currState != PlayerControllerDemo.ActionState.land)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.land, null);
			}
			else
			{
				this.animator.SetTrigger("land");
			}
		}
		else if (this.currState != PlayerControllerDemo.ActionState.passiveAirWithWind)
		{
			if (!this.realAttack)
			{
				this.animator.Play("NoHit");
				base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, "keepAnime");
			}
			else
			{
				base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			}
		}
		else if (!this.realAttack)
		{
			this.animator.SetTrigger("inWind");
		}
	}

	private void AnimeEnd_land()
	{
		if (!this.hasLand)
		{
			base.ChangeState(PlayerControllerDemo.ActionState.passiveAir, null);
			return;
		}
		if (this.currState == PlayerControllerDemo.ActionState.land)
		{
			if (this.inputDir.x != 0f)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.accRun, null);
			}
			else if (this.vel.x == 0f)
			{
				base.ChangeState(PlayerControllerDemo.ActionState.stand, null);
			}
			else
			{
				this.animator.Play("Slide");
				base.ChangeState(PlayerControllerDemo.ActionState.slide, null);
			}
		}
	}

	public void TurnRight()
	{
		this.direction = 1;
		this.myTrans.localScale = new Vector3(1f, 1f, 1f);
	}

	public void OnShowRemainTime(float time)
	{
	}

	private static PlayerControllerDemo _instance;

	private static string pDocument = "this is test information";

	private float _speedInWind;

	private Vector2 accel;

	private bool accelWind;

	private bool actionButtonPressed;

	private Animator animator;

	private float atkAnimationLength;

	public Animator atkAnimator;

	public float attackFireDelay;

	private float attackWindowTime = 0.13333334f;

	private float attackWindowTimer;

	private float audioTimer;

	private bool blockInput;

	private readonly List<IBlowee> bloweeStack = new List<IBlowee>();

	private PlayerCollisionChecker checkerForLand;

	private PlayerCollisionChecker checkerForWall;

	private bool closingToWind;

	public float closingToWindDumpTime = 1f;

	private Vector2 closingWindAccel;

	private Vector2 closingWindVel;

	public Transform controlPointTrans;

	private string currAtkAnimeName;

	private readonly List<Attackable> currAttackableList = new List<Attackable>();

	private Vector2 currContactNormal;

	private XffectComponent currentEffect;

	private bool currrIsLanternKey;

	private ContactPoint2D currWallContactPoint;

	public AnimationClip death2GhostClip;

	public Animator deathAnimator;

	private int deathCount;

	private int direction;

	public bool drawDebugInfo;

	public XffectCache effectPool;

	private float freezeTime;

	public Vector2 gravity;

	private string guid;

	private bool hasLand;

	private bool hasNPC;

	private bool inited;

	public Vector2 initJumpSpeed;

	private Vector2 initWindAccel;

	private Vector2 initWindOffset;

	private Vector2 inputDir = Vector2.zero;

	public PlayerControllerDemo.InputMethod inputMethod;

	public float intoWindAccelFactor = 1f;

	private float intoWindTime;

	private float intoWindTimer;

	public bool invisible;

	private bool isAttack;

	private bool isCollideWithWall;

	private bool isFall;

	private bool isInLantern;

	public Vector2 JumpAccel;

	private float jumpAccelTimer;

	public float jumpDampingFactor;

	private bool landBeWall;

	private bool leftButtonPressed;

	public float maxRunSpeed;

	public float maxYVel;

	public float minYVel;

	private IInputEventHandler myInputEventHandler;

	private Transform myTrans;

	private Quaternion perpQuat = Quaternion.Euler(0f, 0f, 90f);

	private bool realAttack;

	private Vector2 respawnPoint;

	private bool restting;

	private bool rightButtonPressed;

	public float rollYAxisLimit;

	public Transform rotateRootTrans;

	public Vector2 Run2StandAccel;

	private int slideSign;

	public Animator smokeAnimator;

	public Transform smokeTargetTrans;

	public Animator smokeTurnAnimator;

	public Transform smokeTurnTargetTrans;

	public float speedInWind;

	public Vector2 stand2RunAccel;

	private float tempT1;

	private float tempT2;

	private Vector2 tempVec2;

	public bool testing;

	private bool turnDirection;

	private Vector2 vel;

	private Vector2 windOffset;

	private float windTimer;

	private float windWidth;

	private Vector2 worldRight = new Vector2(1f, 0f);

	public enum ActionState
	{
		invalid,
		stand,
		run,
		slide,
		slideTurn,
		accRun,
		preJump,
		jump,
		attack,
		roll,
		land,
		boost,
		passiveAir,
		passiveAirWithWind
	}

	public enum InputMethod
	{
		oldMethod,
		newMethod
	}
}
