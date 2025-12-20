using System;
using ExtensionMethods;
using UnityEngine;

public abstract class EnemyBaseAction : BaseBehaviour
{
	public Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private bool inStiff
	{
		get
		{
			return this.eAttr.stiffTime > 0f;
		}
	}

	protected virtual void Awake()
	{
		this.spineAnim = base.GetComponent<SpineAnimationController>();
		this.eAttr = base.GetComponent<EnemyAttribute>();
		this.stateMachine = base.GetComponent<StateMachine>();
		if (this.shadowPrefab)
		{
			this.shadow = UnityEngine.Object.Instantiate<Transform>(this.shadowPrefab);
			this.shadow.GetComponent<ShadowControl>().SetTarget(base.transform);
		}
	}

	protected abstract void Start();

	protected virtual void Update()
	{
		this.eAttr.stiffTime = Mathf.Clamp(this.eAttr.stiffTime - Time.deltaTime, 0f, float.PositiveInfinity);
		if (this.QTE)
		{
			this.QTE.transform.localScale = ((base.transform.localScale.x >= 0f) ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f));
		}
		if (this.IsInWeakSta() && this.weakEffectShow)
		{
			this.QTEAnim((!this.CurrentCanBeExecute()) ? "Null" : "Show");
		}
		if (!this.IsInNormalState())
		{
			this.isAutoMoveing = false;
		}
		if (!this.isAutoMoveing || SingletonMono<WorldTime>.Instance.IsFrozen)
		{
			return;
		}
		float y = 0f;
		if (this.eAttr.iCanFly)
		{
			float distance = Physics2D.Raycast(base.transform.position + Vector3.right * (float)this.eAttr.faceDir, -Vector2.up, 1000f, LayerManager.GroundMask).distance;
			if (Mathf.Abs(distance - this.eAttr.flyHeight) > 0.1f)
			{
				float num = Mathf.Abs(distance - this.eAttr.flyHeight) / (1f / this.eAttr.moveSpeed);
				y = num * Time.deltaTime * Mathf.Sign(this.eAttr.flyHeight - distance);
			}
		}
		float num2 = Mathf.Abs(this.eAttr.moveSpeed * Time.deltaTime);
		base.transform.position += new Vector3(num2 * (float)this.eAttr.faceDir, y, 0f);
		if (!this.eAttr.iCanFly)
		{
			this.SetToGroundPos();
		}
	}

	public void AnimChangeState(string state, float speed = 1f)
	{
		if (this.eAttr.isDead && !this.IsInDeadState(state))
		{
			return;
		}
		this.animSpeed = speed;
		this.stateMachine.SetState(state);
	}

	public void AnimChangeState(Enum nextState, float speed = 1f)
	{
		this.AnimChangeState(nextState.ToString(), speed);
	}

	public virtual bool IsInNormalState()
	{
		return !this.eAttr.isDead && !this.inStiff && !this.IsInWeakSta();
	}

	public bool CurrentCanBeExecute()
	{
		bool flag = this.IsInWeakSta() && (this.eAttr.rankType == EnemyAttribute.RankType.Normal || (this.eAttr.rankType != EnemyAttribute.RankType.Normal && this.eAttr.isOnGround));
		bool flag2;
		if (R.Player.Attribute.isOnGround)
		{
			flag2 = (this.eAttr.accpectExecute && (this.eAttr.isOnGround || (!this.eAttr.isOnGround && this.eAttr.timeController.GetCurrentSpeed().y < 0f)) && !this.eAttr.isDead);
		}
		else
		{
			flag2 = (this.eAttr.accpectAirExecute && !this.eAttr.isDead);
		}
		bool flag3 = Vector2.Distance(base.transform.position, R.Player.Transform.position) <= 4f;
		return flag && flag2 && flag3;
	}

	public abstract bool IsInDeadState(string state);

	public void ChangeFace(int dir)
	{
		if (this.eAttr.faceDir == dir)
		{
			return;
		}
		this.eAttr.faceDir = dir;
		if (dir != -1)
		{
			if (dir == 1)
			{
				base.transform.localScale = new Vector3(-1f * Mathf.Abs(base.transform.localScale.x), base.transform.localScale.y, 1f);
			}
		}
		else
		{
			base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x), base.transform.localScale.y, 1f);
		}
	}

	public void FaceToPlayer()
	{
		int dir = InputSetting.JudgeDir(base.transform.position, this.player.position);
		this.ChangeFace(dir);
	}

	public bool TurnRound(int dir)
	{
		if (!this.IsInNormalState())
		{
			return false;
		}
		this.ChangeFace(dir);
		return true;
	}

	public virtual void KillSelf()
	{
		if (this.eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			this.eAttr.currentHp = 0;
		}
	}

	public void SetToGroundPos()
	{
		if (this.eAttr.isOnGround)
		{
			return;
		}
		base.transform.position = base.transform.position.SetY(LayerManager.YNum.GetGroundHeight(base.gameObject) + 0.2f);
	}

	public bool AutoMove()
	{
		if (this.isAutoMoveing)
		{
			return true;
		}
		if (this.IsInNormalState())
		{
			this.isAutoMoveing = true;
			this.AnimMove();
			return true;
		}
		return false;
	}

	public bool StopMoveToIdle()
	{
		this.isAutoMoveing = false;
		if (this.IsInNormalState())
		{
			this.AnimReady();
			return true;
		}
		return false;
	}

	public void AppearAtPosition(Vector2 pos)
	{
		base.transform.position = new Vector3(pos.x, pos.y, LayerManager.ZNum.MMiddleE(this.eAttr.rankType));
		this.FaceToPlayer();
	}

	public void AppearEffect(Vector2 pos)
	{
		float num = 3.18f;
		R.Effect.Generate(0, null, new Vector3(pos.x, pos.y + num, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Audio.PlayEffect(123, new Vector3?(base.transform.position));
	}

	public abstract void AnimReady();

	public abstract void AnimMove();

	public virtual void Attack1(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack2(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack3(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack4(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack5(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack6(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack7(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack8(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack9(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack10(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack11(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack12(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack13(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack14(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void Attack15(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.ChangeFace(dir);
	}

	public virtual void CounterAttack(int dir)
	{
	}

	public virtual void Idle1()
	{
		throw new NotImplementedException();
	}

	public virtual void Idle2()
	{
		this.Idle1();
	}

	public virtual void Idle3()
	{
		this.Idle1();
	}

	public virtual void Idle4()
	{
		this.Idle1();
	}

	public virtual void Idle5()
	{
		this.Idle1();
	}

	public virtual void Defence()
	{
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.currentDefence = 0;
		this.eAttr.startDefence = false;
	}

	public virtual void DefenceSuccess()
	{
	}

	public virtual void SideStep()
	{
		this.FaceToPlayer();
		this.eAttr.currentSideStep = 0;
	}

	public void QTEAnim(string anim)
	{
		this.QTESetSkin();
		if (!this.QTE || this.QTE.AnimationName == anim)
		{
			return;
		}
		this.QTESetText(anim);
		this.QTE.state.SetAnimation(0, anim, true);
		this.QTE.skeleton.SetToSetupPose();
	}

	private void QTESetSkin()
	{
		if (this.QTE)
		{
			this.QTE.skeleton.SetSkin((!UILanguage.IsChinese) ? "Mobile" : "Mobile_zh");
		}
	}

	private void QTESetText(string anim)
	{
	}

	public void ExitWeakState(bool forceQuit = false)
	{
		if (!this.eAttr.accpectExecute)
		{
			return;
		}
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.eAttr.willBeExecute || forceQuit)
		{
			this.eAttr.inWeakState = false;
			this.WeakEffectDisappear("RollEnd");
		}
	}

	public void WeakEffectDisappear(string disappear)
	{
		if (!this.eAttr.accpectExecute)
		{
			return;
		}
		if (this.weakThunder)
		{
			this.weakThunder.SetActive(false);
		}
		if (this.weakEffect)
		{
			this.weakEffect.state.SetAnimation(0, disappear, false);
			this.weakEffect.skeleton.SetSlotsToSetupPose();
			this.weakEffect.Update(0f);
		}
		this.weakEffectShow = false;
		this.QTEAnim("Null");
	}

	public virtual void EnterWeakState()
	{
		if (!this.eAttr.accpectExecute)
		{
			return;
		}
		this.eAttr.inWeakState = true;
		this.WeakEffectAppear();
	}

	protected virtual void WeakEffectAppear()
	{
		if (!this.eAttr.accpectExecute)
		{
			return;
		}
		if (this.eAttr.isDead)
		{
			return;
		}
		R.Audio.PlayEffect(104, new Vector3?(base.transform.position));
		if (this.weakThunder)
		{
			this.weakThunder.SetActive(true);
		}
		if (this.weakEffect)
		{
			this.weakEffect.state.SetAnimation(0, "Roll", true);
			this.weakEffect.skeleton.SetSlotsToSetupPose();
			this.weakEffect.Update(0f);
		}
		this.weakEffectShow = true;
		this.QTEAnim("Show");
	}

	public void PlayEffect(int effectId)
	{
		if (!R.Effect.fxData.ContainsKey(effectId))
		{
			Log.Alert(string.Format("Enemy {0} try to play No.{1} Effect, but not exist", base.name, effectId));
			return;
		}
		EnemyAtkEffector component = R.Effect.fxData[effectId].effect.GetComponent<EnemyAtkEffector>();
		Vector3 position = new Vector3(component.pos.x * base.transform.localScale.x, component.pos.y, component.pos.z);
		R.Effect.Generate(effectId, base.transform, position, default(Vector3), default(Vector3), true);
	}

	public virtual bool IsInDefenceState()
	{
		return false;
	}

	public virtual bool IsInSideStepState()
	{
		return false;
	}

	public abstract bool IsInWeakSta();

	public abstract bool IsInAttackState();

	protected abstract bool EnterAtkSta(string lastState, string nextState);

	protected abstract bool ExitAtkSta(string lastState, string nextState);

	public virtual bool IsInIdle()
	{
		return false;
	}

	public virtual void AnimExecute()
	{
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		base.transform.position = position;
	}

	public virtual void AnimQTEHurt()
	{
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		base.transform.position = position;
	}

	public const int LEFT = -1;

	public const int RIGHT = 1;

	public const int STOP = 0;

	[HideInInspector]
	public StateMachine stateMachine;

	[SerializeField]
	public Transform hurtBox;

	[HideInInspector]
	public EnemyAttribute eAttr;

	[SerializeField]
	protected Transform atkBox;

	[SerializeField]
	protected SkeletonAnimation weakEffect;

	[SerializeField]
	protected GameObject weakThunder;

	[SerializeField]
	protected SkeletonAnimation QTE;

	[SerializeField]
	private TextMesh _qteTextMesh;

	protected SpineAnimationController spineAnim;

	protected float animSpeed;

	protected float startDefenceTime;

	protected float defenceTime;

	private bool weakEffectShow;

	public bool isAutoMoveing;

	[SerializeField]
	private Transform shadowPrefab;

	private Transform shadow;
}
