using System;
using System.Collections.Generic;
using GameWorld;
using LitJson;
using UnityEngine;

public class BreakObjHurt : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.currentHP = this.HP;
		this.brokenFlag = false;
		this.cc3 = Camera.main.GetComponent<CameraController>();
		this.pAttr = R.Player.Attribute;
		if (this.breakType == BreakObjHurt.BreakType.SpAttack)
		{
			JsonData jsonData = JsonMapper.ToObject(this.spAttackText.text);
			if (jsonData["spAttack"].IsArray)
			{
				for (int i = 0; i < jsonData["spAttack"].Count; i++)
				{
					this.spAttack.Add(jsonData["spAttack"][i].ToString());
				}
			}
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.Hurt), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.Hurt), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (this.currentHP <= this.breakHp && !this.breakFlag)
		{
			this.breakFlag = true;
			this.sprite.sprite = this.ChooseSprite(this.breakSprite);
		}
		if (this.currentHP <= 0 && !this.brokenFlag)
		{
			this.sprite.sprite = this.ChooseSprite(this.brokenSprite);
			this.brokenFlag = true;
			this.Break();
		}
	}

	private bool Hurt(string eventName, object sender, EnemyHurtAtkEventArgs msg)
	{
		if (msg.hurted != base.gameObject)
		{
			return false;
		}
		BreakObjHurt.BreakType breakType = this.breakType;
		if (breakType != BreakObjHurt.BreakType.HitTimes)
		{
			if (breakType == BreakObjHurt.BreakType.SpAttack)
			{
				this.SpAttackHurt(msg.attackData, msg.hurtPos);
			}
		}
		else
		{
			this.HitTimeHurt(msg.attackData, msg.hurtPos);
		}
		return true;
	}

	private void HitTimeHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
	{
		this.currentHP--;
		Vector3 pos2 = pos - base.transform.position;
		this.HitEffect(pos2);
		this.TimeFrozenAndCameraShake(atkData.frozenFrame, atkData.shakeType, atkData.camShakeFrame, atkData.shakeStrength);
	}

	private void SpAttackHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
	{
		if (this.AvailableAttack(atkData.atkName))
		{
			this.HitTimeHurt(atkData, pos);
		}
	}

	private bool AvailableAttack(string atkName)
	{
		foreach (string b in this.spAttack)
		{
			if (atkName == b)
			{
				return true;
			}
		}
		return false;
	}

	private void Break()
	{
		this.hurtBox.enabled = false;
	}

	private Sprite ChooseSprite(Sprite[] sprite)
	{
		if (sprite.Length <= 0)
		{
			return null;
		}
		int num = UnityEngine.Random.Range(0, sprite.Length);
		return sprite[num];
	}

	protected void TimeFrozenAndCameraShake(int frozenFrame, int shakeType, int shakeFrame, float shakeOffset)
	{
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(frozenFrame, WorldTime.FrozenArgs.FrozenType.All, true);
		if (shakeType != 0)
		{
			if (shakeType != 1)
			{
				if (shakeType == 2)
				{
					this.cc3.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Vertical, false);
				}
			}
			else
			{
				this.cc3.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Horizon, false);
			}
		}
		else
		{
			this.cc3.CameraShake((float)shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Rect, false);
		}
	}

	protected void HitEffect(Vector3 pos)
	{
		R.Effect.Generate(1, base.transform, pos, Vector3.zero, default(Vector3), true);
		R.Effect.Generate(71, base.transform, pos + Vector3.right * (float)this.pAttr.faceDir * 0.5f, new Vector3(0f, (float)((this.pAttr.faceDir <= 0) ? 0 : 180), UnityEngine.Random.Range(-90f, 0f)), default(Vector3), true);
		R.Effect.Generate(151, base.transform, pos, Vector3.zero, default(Vector3), true);
	}

	[Header("破坏条件")]
	public BreakObjHurt.BreakType breakType;

	[Header("总血量")]
	public int HP;

	private int currentHP;

	[Header("破坏条件为特殊攻击时,记录特殊攻击名称，仅在条件为SpAttack时有效")]
	public TextAsset spAttackText;

	private List<string> spAttack = new List<string>();

	public Collider2D hurtBox;

	private bool brokenFlag;

	private bool breakFlag;

	private CameraController cc3;

	private PlayerAttribute pAttr;

	private SpriteRenderer sprite;

	[Header("损坏血量")]
	public int breakHp;

	[Header("损坏图片")]
	public Sprite[] breakSprite;

	[Header("破坏图片")]
	public Sprite[] brokenSprite;

	public enum BreakType
	{
		HitTimes,
		SpAttack
	}
}
