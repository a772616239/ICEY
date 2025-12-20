using System;
using ExtensionMethods;
using LitJson;
using UnityEngine;

public class SoftCollidedImpact : BaseBehaviour
{
	private void Start()
	{
		this._parent = base.transform.parent;
		this.colliderSize = base.GetComponent<Collider2D>().bounds.size;
		this._physics = this._parent.GetComponent<IPlatformPhysics>();
		this._defautValue = this.defautValue;
		this.isOpen = this._defautValue;
		this.state = this._parent.GetComponent<StateMachine>();
		if (this.myType != SoftCollidedImpact.Type.Player)
		{
			this.EnemyAction = this._parent.GetComponent<EnemyBaseAction>();
		}
		if (this.softCollidedParam != null)
		{
			this.softCollidedData = JsonMapper.ToObject(this.softCollidedParam.text);
		}
		this.state.OnTransfer += this.OnStateTransfer;
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		bool flag = this._defautValue;
		if (this.softCollidedData != null && this.softCollidedData.Contains(args.nextState))
		{
			flag = this.softCollidedData[args.nextState].Get<bool>("isOpen", this._defautValue);
		}
		this.isOpen = flag;
	}

	private void FixedUpdate()
	{
		if (this.center != null && base.transform.localPosition != this.center.localPosition)
		{
			base.transform.localPosition = this.center.localPosition;
		}
		int num = Physics2D.OverlapBoxNonAlloc(base.transform.position, this.colliderSize, 0f, this._others, LayerManager.ColliderimpactMask);
		if (num <= 1)
		{
			return;
		}
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			SoftCollidedImpact component = this._others[i].GetComponent<SoftCollidedImpact>();
			if (!(this._others[i].transform == base.transform) && this.Active(component))
			{
				if (this.myType == SoftCollidedImpact.Type.Player)
				{
					num2 = this.Impact(component);
				}
				else
				{
					num2 += this.Impact(component);
				}
			}
		}
		Vector3 position = this._parent.position;
		position.x = this._parent.position.x + num2;
		this._parent.position = position;
	}

	private bool Active(SoftCollidedImpact other)
	{
		return (this.myType != SoftCollidedImpact.Type.Player) ? this.EnemyEnemyActive(other) : this.PlayerEnemyActive(other);
	}

	private float Impact(SoftCollidedImpact other)
	{
		return (this.myType != SoftCollidedImpact.Type.Player) ? this.EnemyEnemyImpact(other) : this.PlayerEnemyImpact(other);
	}

	private bool PlayerEnemyActive(SoftCollidedImpact other)
	{
		if (other.myType != SoftCollidedImpact.Type.Player && other.EnemyAction.eAttr.isDead)
		{
			return false;
		}
		if (this.softCollidedData != null && this.softCollidedData.Contains(this.state.currentState))
		{
			return this.isOpen;
		}
		if (this.state.currentState.IsInArray(PlayerAction.NormalSta))
		{
			return other.myType != SoftCollidedImpact.Type.Small && other.isOpen;
		}
		if (this.state.currentState.IsInArray(PlayerAction.FlySta))
		{
			return other.myType != SoftCollidedImpact.Type.Small && other.isOpen;
		}
		return this.state.currentState.IsInArray(PlayerAction.AirLightAttackSta) || (this.state.currentState.IsInArray(PlayerAction.AttackSta) && (other.myType != SoftCollidedImpact.Type.Small || other.isOpen));
	}

	private float PlayerEnemyImpact(SoftCollidedImpact softCollidedImpact)
	{
		float num = (this.colliderSize.x + softCollidedImpact.colliderSize.x - 0.1f) / 2f;
		float num2 = Mathf.Abs(base.transform.position.x - softCollidedImpact.transform.position.x);
		if (softCollidedImpact.myType > this.myType)
		{
			int num3 = InputSetting.JudgeDir(softCollidedImpact.transform.position, base.transform.position);
			Vector2 velocity = this._physics.velocity;
			if (velocity.x * (float)num3 < 0f)
			{
				velocity.x = 0f;
				this._physics.velocity = velocity;
			}
			return Mathf.Clamp(num - num2, 0f, float.MaxValue) * (float)num3;
		}
		if (softCollidedImpact.myType < this.myType)
		{
			int num4 = InputSetting.JudgeDir(base.transform.position, softCollidedImpact.transform.position);
			float num5 = Mathf.Clamp(num - num2, 0f, float.MaxValue) * (float)num4;
			Vector3 position = softCollidedImpact._parent.position;
			position.x = softCollidedImpact._parent.position.x + num5;
			softCollidedImpact._parent.position = position;
			Vector2 velocity2 = softCollidedImpact._physics.velocity;
			if (velocity2.x * (float)num4 < 0f)
			{
				velocity2.x = 0f;
				softCollidedImpact._physics.velocity = velocity2;
			}
		}
		return 0f;
	}

	private bool EnemyEnemyActive(SoftCollidedImpact other)
	{
		return other.myType != SoftCollidedImpact.Type.Player && this.EnemyAction.IsInIdle() && other.EnemyAction.IsInIdle();
	}

	private float EnemyEnemyImpact(SoftCollidedImpact other)
	{
		float num = (this.colliderSize.x + other.colliderSize.x) / 8f;
		float num2 = Mathf.Abs(base.transform.position.x - other.transform.position.x);
		if (other.myType > this.myType)
		{
			int num3 = InputSetting.JudgeDir(other.transform.position, base.transform.position);
			return Mathf.Clamp(num - num2, 0f, float.MaxValue) * (float)num3;
		}
		if (other.myType == this.myType)
		{
			int num4 = InputSetting.JudgeDir(other.transform.position, base.transform.position);
			return Mathf.Clamp(num - num2, 0f, float.MaxValue) * (float)num4 / 2f;
		}
		return 0f;
	}

	[Header("是否开启，如果为true则开启排斥功能")]
	public bool isOpen = true;

	public SoftCollidedImpact.Type myType;

	public EnemyBaseAction EnemyAction;

	[Header("默认值")]
	public bool defautValue;

	private bool _defautValue;

	private Vector2 colliderSize;

	private IPlatformPhysics _physics;

	private StateMachine state;

	[SerializeField]
	private Transform center;

	[SerializeField]
	private TextAsset softCollidedParam;

	private JsonData softCollidedData;

	private Collider2D[] _others = new Collider2D[20];

	private Transform _parent;

	public enum Type
	{
		Big = 3,
		Player = 2,
		Medium = 1,
		Small = 0
	}
}
