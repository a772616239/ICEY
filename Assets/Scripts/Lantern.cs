using System;
using System.Collections;
using CIS;
using DG.Tweening;
using UnityEngine;

public class Lantern : Attackable, IBlowable, ILevelItem
{
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		if (this.animator != null)
		{
			this.animator.StopPlayback();
		}
		this.myTrans = base.transform;
		this.integrator = base.GetComponent<Integrator>();
		this.sinmove = base.GetComponent<SinMove>();
		this.maxSpeedSquare = this.maxSpeedInWind * this.maxSpeedInWind;
		this.areaTriggered = !this.needArearTrigger;
	}

	protected override void Start()
	{
		base.Start();
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (this.integrator == null)
		{
			return;
		}
		if (coll.CompareTag("Wall") || coll.CompareTag("Land"))
		{
			this.colldeNormal = -this.integrator.vel.normalized;
			this.integrator.vel = this.colldeNormal * UnityEngine.Random.Range(0.4f, 0.5f);
			this.isCollideWithWall = true;
		}
	}

	private void OnTriggerExit2D(Collider2D coll)
	{
		if (this.integrator == null)
		{
			return;
		}
		if (coll.CompareTag("Wall") || coll.CompareTag("Land"))
		{
			this.isCollideWithWall = false;
		}
	}

	public override void Update()
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		if (!this.areaTriggered)
		{
			return;
		}
		base.Update();
		if (this.beBlowed)
		{
			Vector2 vector = this.myTrans.position;
			if (this.currBlowee != null)
			{
				Vector2 vector2 = this.currBlowee.GetDir(this, vector) * this.currBlowee.GetPower(this, vector) * this.windAffectFactor + this.dampingInWindAccel;
				if (this.isCollideWithWall)
				{
					vector2 = -vector2 * UnityEngine.Random.Range(0.6f, 0.8f);
					if ((this.integrator.vel + vector2 * Time.deltaTime).sqrMagnitude > this.maxSpeedSquare)
					{
						vector2 *= 0.5f;
					}
				}
				this.integrator.accel = vector2;
				if (this.dampingInWindTimer == 0f && this.integrator.vel.sqrMagnitude > this.maxSpeedSquare)
				{
					this.integrator.accel = Vector2.zero;
				}
			}
			this.myTrans.position = this.integrator.Integrate(vector);
			Vector2 normalized = this.integrator.vel.normalized;
			if (this.currBlowee == null && Vector2.Dot(normalized, this.lastVel) < 0f)
			{
				this.beBlowed = false;
				this.sinmove.SetInitPos(vector);
				UnityEngine.Debug.Log("blowed outsied");
				this.myTrans.DOMove(this.sinmove.GetCurrentPos(), 0.5f, false).OnComplete(delegate
				{
					this.sinmove.enabled = true;
				});
			}
			else if (this.dampingInWindTimer > 0f)
			{
				this.dampingInWindTimer -= SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			}
			else if (this.dampingInWindTimer < 0f)
			{
				Vector2 a = Vector3.Project(this.integrator.vel, this.intoWindDir);
				this.integrator.vel += -a;
				this.dampingInWindTimer = 0f;
				this.dampingInWindAccel = Vector2.zero;
			}
			this.lastVel = normalized;
		}
	}

	private IEnumerator _PlayAnimation(string name, float time)
	{
		yield return new WaitForSeconds(time);
		if (this.lightAnimator1 != null)
		{
			this.animator.Play("hit3");
		}
		float z = UnityEngine.Random.Range(-40f, 40f);
		if (this.lightAnimator1 != null)
		{
			this.lightAnimator1.transform.rotation = Quaternion.Euler(0f, 0f, z);
			this.lightAnimator1.Play("lanternLight");
		}
		if (this.lightAnimator2 != null)
		{
			this.lightAnimator2.transform.rotation = Quaternion.Euler(0f, 0f, z + UnityEngine.Random.Range(50f, 110f));
			this.lightAnimator2.Play("lanternLightAlter");
		}
		if (this.lightBackground != null)
		{
			this.lightBackground.Play("lanternLightBackground");
		}
		if (this.particleEffectPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particleEffectPrefab);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector2.zero;
			gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			gameObject.transform.position = this.upperFireTarget.position;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.particleEffectPrefab);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector2.zero;
			gameObject2.transform.rotation = Quaternion.Euler(0f, 180f, -180f);
			gameObject2.transform.position = this.bottomFireTarget.position;
		}
		yield break;
	}

	public override void PlayAnimationAfterTime(string name, float time)
	{
		if (this.animator != null)
		{
			base.StartCoroutine(this._PlayAnimation(name, time));
		}
	}

	public override void React(Vector2 dir, float delay = 0f)
	{
		if (this.beBlowed)
		{
			return;
		}
		base.React(dir, 0f);
		SinMove sinMove = base.GetComponent<SinMove>();
		bool sinEnabled = sinMove.enabled;
		sinMove.enabled = false;
		Vector2 vector = this.myTrans.position;
		Vector2 v = vector + dir.normalized * (float)UnityEngine.Random.Range(25, 30);
		Sequence s = DOTween.Sequence();
		this.available = false;
		s.AppendInterval(delay);
		s.Append(this.myTrans.DOMove(v, 0.2f, false));
		s.Append(this.myTrans.DOMove(vector, 0.3f, false));
		s.AppendCallback(delegate
		{
			sinMove.enabled = sinEnabled;
			this.available = true;
		});
	}

	public override bool Available()
	{
		return this.available;
	}

	public void OnReset()
	{
		if (this.integrator != null)
		{
			this.integrator.vel = Vector2.zero;
			this.integrator.accel = Vector2.zero;
			this.lastVel = Vector2.zero;
			this.intoWindDir = Vector2.zero;
			this.dampingInWindAccel = Vector2.zero;
			this.dampingInWindTimer = 0f;
		}
		if (this.sinmove != null && !this.sinmove.enabled)
		{
			this.sinmove.OnReset();
		}
		this.available = true;
		this.areaTriggered = !this.needArearTrigger;
	}

	public void OnPause(bool isPause)
	{
	}

	public void _OnWindEnter(IBlowee wind)
	{
		if (this.integrator == null)
		{
			return;
		}
		this.sinmove.enabled = false;
		this.currBlowee = wind;
		this.beBlowed = true;
		Vector2 vector = wind.GetGO().transform.position - this.myTrans.position;
		Vector2 vel = this.integrator.vel;
		float a;
		if (vector.x == 0f)
		{
			this.dampingInWindAccel.x = 0f;
			a = 0f;
		}
		else
		{
			this.dampingInWindAccel.x = -(vel.x * vel.x / (2f * vector.x));
			if (this.dampingInWindAccel.x == 0f)
			{
				a = 0f;
			}
			else
			{
				a = Mathf.Abs(vel.x / this.dampingInWindAccel.x);
			}
		}
		float b;
		if (vector.y == 0f)
		{
			this.dampingInWindAccel.y = 0f;
			b = 0f;
		}
		else
		{
			this.dampingInWindAccel.y = -(vel.y * vel.y / (2f * vector.y));
			if (this.dampingInWindAccel.y == 0f)
			{
				b = 0f;
			}
			else
			{
				b = Mathf.Abs(vel.y / this.dampingInWindAccel.y);
			}
		}
		this.dampingInWindTime = Mathf.Max(a, b);
		this.dampingInWindTimer = this.dampingInWindTime;
		this.intoWindDir = this.integrator.vel.normalized;
	}

	public void _OnWindExit(IBlowee wind)
	{
		if (this.integrator == null)
		{
			return;
		}
		if (this.currBlowee == null || wind != this.currBlowee)
		{
			return;
		}
		Vector2 vector = this.myTrans.position;
		if (this.blowPinTargetTrans == null)
		{
			this.integrator.accel = -wind.GetDir(this, vector) * wind.GetPower(this, vector) * this.dampingFactor;
			Vector2 a = Vector3.Project(this.integrator.vel, this.intoWindDir);
			this.integrator.vel += -a;
		}
		else
		{
			Vector2 a2 = this.blowPinTargetTrans.position;
			Vector2 vector2 = a2 - vector;
			Vector2 vel = this.integrator.vel;
			if (vector2.x == 0f)
			{
				this.integrator.accel.x = 0f;
			}
			else
			{
				this.integrator.accel.x = -(vel.x * vel.x / (2f * vector2.x));
			}
			if (vector2.y == 0f)
			{
				this.integrator.accel.y = 0f;
			}
			else
			{
				this.integrator.accel.y = -(vel.y * vel.y / (2f * vector2.y));
			}
		}
		this.currBlowee = null;
	}

	public Bounds GetBounds()
	{
		return default(Bounds);
	}

	public void OnAreaTriggerEnter(Collider2D coll)
	{
		if (coll.CompareTag("Player"))
		{
			this.areaTriggered = true;
		}
	}

	public void OnAreaTriggerExit(Collider2D coll)
	{
	}

	public void OnEntertBlock(ILevelBlock block)
	{
	}

	public void OnLeaveBlock(ILevelBlock block)
	{
	}

	public Animator lightAnimator1;

	public Animator lightAnimator2;

	public Animator lightBackground;

	public GameObject particleEffectPrefab;

	public Transform upperFireTarget;

	public Transform bottomFireTarget;

	public float windAffectFactor = 1f;

	public float dampingFactor = 1f;

	public Transform blowPinTargetTrans;

	public float maxSpeedInWind;

	public bool needArearTrigger;

	private bool areaTriggered = true;

	private float maxSpeedSquare;

	private Animator animator;

	private Transform myTrans;

	private Integrator integrator;

	private SinMove sinmove;

	private IBlowee currBlowee;

	private bool beBlowed;

	private bool isCollideWithWall;

	private Vector2 colldeNormal;

	private Vector2 dampingInWindAccel;

	private float dampingInWindTime;

	private float dampingInWindTimer;

	private Vector2 intoWindDir;

	private bool available = true;

	private Vector2 lastVel = Vector2.zero;
}
