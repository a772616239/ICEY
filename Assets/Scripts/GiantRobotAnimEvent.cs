using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

public class GiantRobotAnimEvent : BaseBehaviour
{
	private GameObject player
	{
		get
		{
			return R.Player.GameObject;
		}
	}

	private void Awake()
	{
		this._action = base.GetComponent<GiantRobotAction>();
		this._eAttr = base.GetComponentInChildren<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
		this.bodyLower = GameObject.Find("BodyLower");
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.巨型机器人];
	}

	public void ChangeState(GiantRobotAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void Attack2Effect()
	{
		R.Effect.Generate(133, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(145, base.transform, new Vector2(1.2f, 1f), default(Vector3), default(Vector3), true);
	}

	public void NormalLaserAttack()
	{
		this.halo_L.SetActive(true);
		this.halo_R.SetActive(true);
		this.NormalLaser(true);
		this.NormalLaser(false);
	}

	private void NormalLaser(bool left)
	{
		Vector2 v = (!left) ? (this.laser_R_F.position - this.laser_R.position) : (this.laser_L_F.position - this.laser_L.position);
		Transform transform = R.Effect.Generate(204, (!left) ? this.laser_R_F : this.laser_L_F, default(Vector3), default(Vector3), default(Vector3), true);
		Laser component = transform.GetComponent<Laser>();
		component.AppearLaser(v, base.gameObject, true);
		component.SetAtkData(this._jsonData[this._action.stateMachine.currentState], this._eAttr.atk);
	}

	public void LaserAttack(int frame)
	{
		this.halo_L.SetActive(true);
		this.halo_R.SetActive(true);
		base.StartCoroutine(this.LaserRotate(this.laser_L_F, true, frame));
		base.StartCoroutine(this.LaserRotate(this.laser_R_F, false, frame));
	}

	private IEnumerator LaserRotate(Transform initPos, bool left, int frame)
	{
		Vector2 dir = (!left) ? (this.laser_R_F.position - this.laser_R.position) : (this.laser_L_F.position - this.laser_L.position);
		Transform laser = R.Effect.Generate(203, initPos, default(Vector3), default(Vector3), default(Vector3), true);
		laser.parent = initPos;
		Laser i = laser.GetComponent<Laser>();
		i.AppearLaser(dir, base.gameObject, false);
		i.SetAtkData(this._jsonData[this._action.stateMachine.currentState], this._eAttr.atk);
		yield return base.StartCoroutine(laser.GetComponent<Laser>().LaserFadeIn());
		for (int j = 0; j < frame; j++)
		{
			if (this._action.IsInWeakSta())
			{
				break;
			}
			i.Rotate(this.GetLaserDir(left));
			yield return null;
		}
		yield return base.StartCoroutine(laser.GetComponent<Laser>().LaserFadeOut());
		UnityEngine.Object.Destroy(laser.gameObject);
		this.DisappearHalo();
		yield break;
	}

	private Vector2 GetLaserDir(bool left)
	{
		return (!left) ? (this.laser_R_F.position - this.laser_R.position) : (this.laser_L_F.position - this.laser_L.position);
	}

	public void DisappearHalo()
	{
		this.halo_L.SetActive(false);
		this.halo_R.SetActive(false);
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(GiantRobotAction.StateEnum.ReadyToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(GiantRobotAction.StateEnum.Ready, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 115; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(GiantRobotAction.StateEnum.WeakModToReady, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void CameraShake(int frame)
	{
		R.Camera.Controller.CameraShake((float)frame / 60f, 0.1f, CameraController.ShakeTypeEnum.Vertical, false);
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void DestroySelf()
	{
		R.Enemy.RemoveEnemy(this._eAttr);
	}

	public void DestroyBodyLower()
	{
		if (this.bodyLower != null)
		{
			UnityEngine.Object.Destroy(this.bodyLower);
		}
	}

	public void QTEDieShadeAtkHitGround(int times)
	{
		Transform hurtPos = this.GetHurtPos(times);
		int num = InputSetting.JudgeDir(base.transform.position, hurtPos.position);
		Transform transform = R.Effect.Generate(180, null, new Vector3(hurtPos.position.x, base.transform.position.y, 0f), default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x *= (float)(num * -1);
		transform.localScale = localScale;
	}

	public void QTEDieEffect(int times)
	{
		Transform hurtPos = this.GetHurtPos(times);
		GiantRobotHurt component = base.GetComponent<GiantRobotHurt>();
		component.HitEffect(hurtPos.localPosition, "Atk1");
		SingletonMono<CameraController>.Instance.CameraShake(0.06666667f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void ArmFallDown()
	{
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
	}

	public void QTEDiePlayerHitGround()
	{
		this.player.transform.position += Vector3.up * 6f;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEHitGround, 1f);
	}

	public void CameraZoomIn()
	{
		SingletonMono<CameraController>.Instance.CameraZoom(base.transform.position + new Vector3(2f, 4f, 0f), 0.5f, 3f);
	}

	public void QTEDiePlayerPush()
	{
		this.player.transform.position += new Vector3(2f, 4f, 0f);
		R.Player.Action.TurnRound(-1);
		R.Player.Action.ChangeState(PlayerAction.StateEnum.AirQTEPush, 1f);
	}

	public void QTEDieFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", this.player.gameObject, args);
	}

	private Transform GetHurtPos(int times)
	{
		switch (times)
		{
		case 1:
			return this.RForeArm;
		case 2:
			return this.RArm;
		case 3:
			return this.LForeArm;
		case 4:
			return this.LArm;
		default:
			return this.Center;
		}
	}

	private GiantRobotAction _action;

	private EnemyAttribute _eAttr;

	private JsonData _jsonData;

	[SerializeField]
	private Transform LArm;

	[SerializeField]
	private Transform LForeArm;

	[SerializeField]
	private Transform RArm;

	[SerializeField]
	private Transform RForeArm;

	[SerializeField]
	private Transform Center;

	[SerializeField]
	private Transform headPos;

	[SerializeField]
	private Transform headPrefab;

	[SerializeField]
	private Transform laser_L;

	[SerializeField]
	private Transform laser_L_F;

	[SerializeField]
	private Transform laser_R;

	[SerializeField]
	private Transform laser_R_F;

	[SerializeField]
	private GameObject bodyLower;

	public GameObject halo_L;

	public GameObject halo_R;

	private EnemyAtk _enemyAtk;
}
