using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class SpiderBossAnimEvent : BaseBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Awake()
	{
		this._action = base.GetComponent<SpiderBossAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
		this._baseHurt = base.GetComponent<EnemyBaseHurt>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.愚笨蜘蛛];
	}

	public void ChangeState(SpiderBossAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this.SetAtkId();
	}

	public void SetAtkId()
	{
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void SummonSpider()
	{
		base.StartCoroutine(this.SummonSpiderCoroutine());
	}

	private IEnumerator SummonSpiderCoroutine()
	{
		int count = (R.GameData.Difficulty > 1) ? 5 : 4;
		for (int i = 0; i < count; i++)
		{
			if (this._action.stateMachine.currentState != "Atk4")
			{
				yield break;
			}
			Vector2 pos = new Vector2(UnityEngine.Random.Range(GameArea.EnemyRange.xMin + 3f, GameArea.EnemyRange.xMax - 3f), base.transform.position.y);
			GameObject spider = Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.蜘蛛, new Vector2?(pos), true, true);
			spider.GetComponent<EnemyAttribute>().playerInView = true;
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(SpiderBossAction.StateEnum.IdleToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(SpiderBossAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 115; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(SpiderBossAction.StateEnum.WeakModToIdle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void DestroySelf()
	{
		R.Enemy.RemoveEnemy(this._eAttr);
	}

	public void QTEHurtShadeAtk()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)this._eAttr.faceDir;
		transform.localScale = one;
	}

	public void QTEHurtShadeAtkBack()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)(-(float)this._eAttr.faceDir);
		transform.localScale = one;
	}

	public void QTEHurtPlayerHitGround()
	{
		this.player.position = base.transform.position + Vector3.up * 5f + Vector3.right * (float)this._eAttr.faceDir * 1.5f;
		int dir = InputSetting.JudgeDir(this.player.transform.position, base.transform.position);
		R.Player.Action.TurnRound(dir);
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEHitGround, 1f);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EventManager.PostEvent<Transform, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player, args);
	}

	public void QTEEffect()
	{
		this._baseHurt.HitEffect(this._baseHurt.center.localPosition, "Atk1");
		R.Camera.Controller.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
	}

	public void QTEDieShadeAtk(int times)
	{
		Vector3 one = Vector3.one;
		switch (times)
		{
		case 1:
		{
			Transform transform = R.Effect.Generate(177, null, this.leg1.position, default(Vector3), default(Vector3), true);
			one.x *= (float)this._eAttr.faceDir;
			transform.localScale = one;
			break;
		}
		case 2:
		{
			Transform transform = R.Effect.Generate(177, null, this.leg1.position, default(Vector3), default(Vector3), true);
			one.x *= (float)(-(float)this._eAttr.faceDir);
			transform.localScale = one;
			break;
		}
		case 3:
		{
			Transform transform = R.Effect.Generate(177, null, this.leg1.position, default(Vector3), default(Vector3), true);
			one.x *= (float)this._eAttr.faceDir;
			transform.localScale = one;
			break;
		}
		case 4:
		{
			Transform transform = R.Effect.Generate(177, null, this.leg1.position, default(Vector3), default(Vector3), true);
			one.x *= (float)(-(float)this._eAttr.faceDir);
			transform.localScale = one;
			break;
		}
		}
	}

	public void QTEDieEffect(int times)
	{
		R.Audio.PlayEffect(255, new Vector3?(base.transform.position));
		switch (times)
		{
		case 1:
			this._baseHurt.HitEffect(this.leg1.localPosition, "Atk1");
			break;
		case 2:
			this._baseHurt.HitEffect(this.leg2.localPosition, "Atk1");
			break;
		case 3:
			this._baseHurt.HitEffect(this.leg3.localPosition, "Atk1");
			break;
		case 4:
			this._baseHurt.HitEffect(this.leg4.localPosition, "Atk1");
			break;
		}
		R.Camera.Controller.CameraShake(0.2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void QTEDieHitGround()
	{
		this.player.transform.position = base.transform.position + Vector3.up * 5f + Vector3.right * (float)this._eAttr.faceDir * 1.5f;
		int dir = InputSetting.JudgeDir(this.player.transform.position, base.transform.position);
		R.Player.Action.TurnRound(dir);
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEHitGround, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	private SpiderBossAction _action;

	private EnemyAttribute _eAttr;

	private JsonData _jsonData;

	[SerializeField]
	private Transform leg1;

	[SerializeField]
	private Transform leg2;

	[SerializeField]
	private Transform leg3;

	[SerializeField]
	private Transform leg4;

	private EnemyAtk _enemyAtk;

	private EnemyBaseHurt _baseHurt;
}
