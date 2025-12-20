using System;
using DatabaseModel;
using UnityEngine;

public class EnemyAttribute : BaseBehaviour
{
	public float viewXFront
	{
		get
		{
			return this._viewXFront;
		}
		set
		{
			this._viewXFront = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	public float viewXBack
	{
		get
		{
			return this._viewXBack;
		}
		set
		{
			this._viewXBack = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	public float viewYTop
	{
		get
		{
			return this._viewYTop;
		}
		set
		{
			this._viewYTop = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	public float viewYDown
	{
		get
		{
			return this._viewYDown;
		}
		set
		{
			this._viewYDown = Mathf.Clamp(value, 0f, float.MaxValue);
		}
	}

	public float currentFlyHeight
	{
		get
		{
			return MathfX.PosToHeight(base.transform.position);
		}
	}

	public int currentHp
	{
		get
		{
			return this._currentHp;
		}
		set
		{
			this._currentHp = Mathf.Clamp(value, 0, this.maxHp);
		}
	}

	public int currentSp
	{
		get
		{
			return this._currentSp;
		}
		set
		{
			this._currentSp = Mathf.Clamp(value, 0, this.maxSP);
		}
	}

	public bool canCounterAttack
	{
		get
		{
			return this.counterAttack > 0;
		}
	}

	public bool hasActionInterrupt
	{
		get
		{
			return this.actionInterruptPoint > 0;
		}
	}

	public int monsterDefence
	{
		get
		{
			return (int)((float)this.dynamicDefence * this.hardLevel);
		}
	}

	public int monsterSideStep
	{
		get
		{
			return (int)((float)this.dynamicSideStep * this.hardLevel);
		}
	}

	public bool isDead
	{
		get
		{
			return this.currentHp <= 0;
		}
	}

	[HideInInspector]
	public bool isArmorBroken
	{
		get
		{
			return this.currentSp <= 0;
		}
	}

	public bool isOnGround
	{
		get
		{
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, -Vector2.up, this.altitude, LayerManager.ObstacleMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask);
			RaycastHit2D hit2 = Physics2D.Raycast(base.transform.position + Vector3.right * this.bounds.size.x / 2f, -Vector2.up, this.altitude, LayerManager.ObstacleMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask);
			RaycastHit2D hit3 = Physics2D.Raycast(base.transform.position - Vector3.right * this.bounds.size.x / 2f, -Vector2.up, this.altitude, LayerManager.ObstacleMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask);
			return hit || hit2 || hit3;
		}
	}

	public float height
	{
		get
		{
			return Physics2D.Raycast(base.transform.position, Vector2.up * -1f, 100f, LayerManager.ObstacleMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask).distance;
		}
	}

	public bool CurrentCanBeExecute
	{
		get
		{
			if (this._action == null)
			{
				this._action = base.GetComponent<EnemyBaseAction>();
			}
			return this._action.CurrentCanBeExecute();
		}
	}

	protected virtual void Awake()
	{
		this.timeController = base.GetComponent<TimeController>();
		this.currentSp = this.maxSP;
		this.currentHp = this.maxHp;
	}

	protected virtual void Start()
	{
		if (this.enemyGravity != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.enemyGravity);
			gameObject.transform.parent = base.transform;
		}
		this.dynamicDefence = (int)((float)this.baseDefence * UnityEngine.Random.Range(0.7f, 1.3f));
		this.dynamicSideStep = (int)((float)this.baseSideStep * UnityEngine.Random.Range(0.7f, 1.3f));
		this.bounds = base.GetComponent<Collider2D>().bounds;
		if (this.rankType == EnemyAttribute.RankType.BOSS)
		{
			this.SetAsBoss();
		}
	}

	protected virtual void Update()
	{
	}

	protected void OnEnable()
	{
		if (this.InTheWorld)
		{
			R.Enemy.AddEnemy(this);
		}
	}

	protected void OnDisable()
	{
		if (this.InTheWorld)
		{
			R.Enemy.RemoveEnemy(this);
		}
	}

	public void SetBasicData(EnemyAttrData baseData)
	{
		float num = 1f;
		switch (R.GameData.Difficulty)
		{
		case 0:
			num = 0.7f;
			break;
		case 1:
			num = 1f;
			break;
		case 2:
		case 3:
			num = 1.5f;
			break;
		}
		float num2 = (R.GameData.Difficulty != 0) ? 1.2f : 1f;
		float num3 = (R.GameData.Difficulty != 1) ? 1.5f : 1f;
		this.baseData = baseData;
		this.maxHp = (int)((float)baseData.maxHp * num2);
		this.atk = (int)((float)baseData.atk * num);
		this.counterAttack = (int)((float)baseData.counterAttack * num3);
		this.level = baseData.level;
		this.maxSP = baseData.maxSP;
		this.atkSpeed = baseData.atkSpeed;
		this.scanSpeed = baseData.scanSpeed;
		this.moveSpeed = baseData.moveSpeed;
		this.flyHeight = MathfX.HeightToPos((float)baseData.flyHeight);
		this.counterAttackProbPercentage = baseData.counterAttackProbPercentage;
		this.actionInterruptPoint = baseData.actionInterruptPoint;
		this.currentHp = this.maxHp;
		this.currentSp = baseData.maxSP;
		this.currentActionInterruptPoint = 0;
		this.currentCounterAttackValue = 0;
		this.currentCounterAttackProbPercentage = (float)baseData.counterAttackProbPercentage;
		this.dropCoins = baseData.dropCoins;
		this.dropExp = baseData.dropExp;
	}

	public void SetAsBoss()
	{
		R.Enemy.Boss = base.gameObject;
		R.Ui.bossHpBar.Create(base.GetComponent<EnemyAttribute>(), base.GetComponent<EnemyBaseHurt>().phaseHp);
	}

	public EnemyAttribute.RankType rankType;

	public Bounds bounds;

	[SerializeField]
	private float _viewXFront;

	[SerializeField]
	private float _viewXBack;

	[SerializeField]
	private float _viewYTop;

	[SerializeField]
	private float _viewYDown;

	public bool playerInView;

	[Header("敌人关卡基本属性值，编辑器修改无效")]
	public EnemyAttrData baseData;

	public string id;

	[Header("等级值")]
	public int level = 1;

	[Header("最大护盾值")]
	public int maxSP;

	[Header("最大血量")]
	public int maxHp = 1000;

	[Header("攻击力")]
	public int atk;

	[Header("攻击动画速率")]
	public float atkSpeed = 1f;

	[Header("扫描速度 ")]
	public float scanSpeed = 2f;

	[Header("移动速度")]
	public float moveSpeed = 4f;

	[Header("掉落金币数量")]
	public int dropCoins;

	[Header("经验值")]
	public int dropExp;

	[Header("飞行高度，指的是Y值")]
	public float flyHeight;

	[Header("表示怪物能不能飞")]
	public bool iCanFly;

	[SerializeField]
	private int _currentHp;

	[SerializeField]
	private int _currentSp;

	[Header("反击值")]
	public int counterAttack;

	[Header("当前反击值")]
	public int currentCounterAttackValue;

	[Header("当前反击几率")]
	public float currentCounterAttackProbPercentage;

	[Header("反击触发概率值")]
	[Range(0f, 100f)]
	public int counterAttackProbPercentage;

	[Header("硬直值")]
	public int actionInterruptPoint;

	[Header("当前的硬直值")]
	public int currentActionInterruptPoint;

	[Header("基础防御值")]
	public int baseDefence;

	[Header("基础闪避值")]
	public int baseSideStep;

	[HideInInspector]
	public int dynamicDefence;

	[HideInInspector]
	public int dynamicSideStep;

	[Header("难度系数")]
	public float hardLevel;

	[Header("防御累计伤害")]
	public int currentDefence;

	[Header("开始防御")]
	public bool startDefence;

	[Header("闪避累计伤害")]
	public int currentSideStep;

	[Header("空中闪避")]
	public bool sideStepInAir;

	[Header("是否在霸体")]
	public bool paBody;

	[Header("是否在虚弱状态中")]
	public bool inWeakState;

	[Header("是否加入世界数据")]
	public bool InTheWorld = true;

	public int faceDir = -1;

	public float stiffTime;

	public const int LEFT = -1;

	public const int RIGHT = 1;

	public bool accpectExecute;

	public bool accpectAirExecute;

	public bool enterWeakMod;

	public bool willBeExecute;

	public bool canBeChased;

	[HideInInspector]
	public bool isFlyingUp;

	public bool followLeftHand;

	[HideInInspector]
	public bool checkHitGround;

	[HideInInspector]
	public bool flyToFall;

	public float altitude = 0.2f;

	[NonSerialized]
	private EnemyBaseAction _action;

	[HideInInspector]
	public TimeController timeController;

	public GameObject enemyGravity;

	public enum RankType
	{
		Normal,
		Elite,
		BOSS
	}
}
