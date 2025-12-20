using System;
using System.Collections;
using GameWorld;
using LitJson;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : BaseBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	public GameObject orgin
	{
		get
		{
			return this._orgin;
		}
		set
		{
			this._orgin = value;
			this._enemyAttribute = this._orgin.GetComponent<EnemyAttribute>();
		}
	}

	private void Awake()
	{
		this.line = base.GetComponent<LineRenderer>();
		this.line.positionCount = 2;
		Laser._effectiveLayer = (LayerManager.GroundMask | LayerManager.WallMask | LayerManager.CeilingMask);
	}

	private void OnEnable()
	{
		this._interval = 0f;
		this._hit = false;
		this.Init();
	}

	private void Update()
	{
		if (this.orgin == null || !this.orgin.activeSelf || this._enemyAttribute.isDead)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.UpdatePoint();
		this._interval -= Time.deltaTime;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(this.data.point1, this.data.point2 - this.data.point1, this.data.laserLength, LayerManager.PlayerBodyMask | Laser._effectiveLayer);
		if (raycastHit2D.collider != null && raycastHit2D.collider.name == "PlayerHurtBox" && this._interval <= 0f)
		{
			this._interval = 0.2f;
			if (!this._hit)
			{
				this._hit = true;
				this.EventTrigger();
			}
			else if (this.data.type == 1)
			{
				this.EventTrigger();
			}
		}
	}

	private void EventTrigger()
	{
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, this.orgin, this.damage, Incrementor.GetNextId(), this.atkData, false);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	private void UpdatePoint()
	{
		Vector3 position = base.transform.position;
		position.z = LayerManager.ZNum.Fx;
		this.data.point1 = position;
	}

	private void Init()
	{
		this.startTime = Time.time;
		this.widthTemp = 0f;
		this.angleTemp = 0f;
		this.line.startWidth = this.data.laserWidth;
		this.line.endWidth = this.data.laserWidth;
	}

	public void ShowLine()
	{
		this.line.SetPosition(0, this.data.point1);
		this.line.SetPosition(1, this.data.point2);
	}

	public void SetStartPoint(Vector3 point1, Vector3 point2)
	{
		this.data.point1 = point1;
		this.data.point2 = point2;
	}

	public void Rotate()
	{
		this.angleTemp += Time.deltaTime * this.data.rotateSpeed;
		Vector3 vector = MathfX.RotateVector(Vector3.up, this.data.startAngle + this.angleTemp);
		this.hit = Physics2D.Raycast(this.data.point1, vector, this.data.laserLength, Laser._effectiveLayer);
		this.data.point2 = this.hit.point;
		if (!this.hit)
		{
			this.data.point2 = this.data.point1 + vector * this.data.laserLength;
		}
		if (this.data.canHitGround)
		{
			base.transform.GetChild(0).position = this.data.point2;
		}
	}

	public void Rotate(Vector3 dir)
	{
		this.hit = Physics2D.Raycast(this.data.point1, dir, this.data.laserLength, Laser._effectiveLayer);
		this.data.point2 = this.hit.point;
		if (!this.hit)
		{
			this.data.point2 = this.data.point1 + dir * this.data.laserLength;
		}
		if (this.data.canHitGround)
		{
			base.transform.GetChild(0).position = this.data.point2;
		}
		this.ShowLine();
	}

	private IEnumerator Run()
	{
		yield return base.StartCoroutine(this.LaserFadeIn());
		this.canRun = true;
		while (this.canRun)
		{
			if (this.data.canRotation && this.deltaTime > this.data.waitTime)
			{
				if (Mathf.Abs(this.data.rotateAngle) >= Mathf.Abs(this.angleTemp) && Vector2.Distance(this.data.point2, this.startPoint2) < Mathf.Abs(this.data.maxDistance))
				{
					this.Rotate();
				}
				else
				{
					this.canRun = false;
					yield return base.StartCoroutine(this.LaserFadeOut());
				}
			}
			this.ShowLine();
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	public void RotateByPoint(float rotateAngle, Vector3 startPoint1, float startAngle, float rotateTime, GameObject orgin)
	{
		EventManager.RegisterEvent<EventArgs>("DestroyEffect", new EventManager.FBEventHandler<EventArgs>(this.DestroyEffect), EventManager.ListenerQueue.Game);
		this.orgin = orgin;
		this.data.waitTime = rotateTime - this.data.appearTime * 2f;
		this.data.startAngle = startAngle;
		this.data.rotateSpeed = rotateAngle / rotateTime;
		Vector3 vector = MathfX.RotateVector(Vector3.up, this.data.startAngle);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(startPoint1, vector, this.data.laserLength, Laser._effectiveLayer);
		this.data.point1 = startPoint1;
		this.data.point2 = raycastHit2D.point;
		if (!raycastHit2D)
		{
			this.data.point2 = this.data.point1 + vector * this.data.laserLength;
		}
		this.startPoint2 = this.data.point2;
		if (this.data.canHitGround)
		{
			base.transform.GetChild(0).position = this.data.point2;
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		base.StartCoroutine(this.Run());
	}

	public IEnumerator LaserFadeIn()
	{
		this.line.startWidth = 0f;
		this.line.endWidth = 0f;
		this.ShowLine();
		for (float i = 0f; i < this.data.appearTime; i += Time.deltaTime)
		{
			this.widthTemp = Mathf.Lerp(0f, this.data.laserWidth, i / this.data.appearTime);
			this.widthTemp = ((this.widthTemp >= 0.05f) ? this.widthTemp : 0f);
			this.line.startWidth = this.widthTemp;
			this.line.endWidth = this.widthTemp;
			yield return null;
		}
		this.line.startWidth = this.data.laserWidth;
		this.line.endWidth = this.data.laserWidth;
		yield break;
	}

	public IEnumerator LaserFadeOut()
	{
		for (float i = 0f; i < this.data.appearTime; i += Time.deltaTime)
		{
			this.widthTemp = Mathf.Lerp(this.data.laserWidth, 0f, i / this.data.appearTime);
			this.widthTemp = ((this.widthTemp >= 0.05f) ? this.widthTemp : 0f);
			this.line.startWidth = this.widthTemp;
			this.line.endWidth = this.widthTemp;
			yield return null;
		}
		base.transform.GetChild(0).gameObject.SetActive(false);
		this.line.startWidth = 0f;
		this.line.endWidth = 0f;
		yield break;
	}

	public void AppearLaser(Vector3 startPoint1, float startAngle, GameObject orgin)
	{
		EventManager.RegisterEvent<EventArgs>("DestroyEffect", new EventManager.FBEventHandler<EventArgs>(this.DestroyEffect), EventManager.ListenerQueue.Game);
		this.orgin = orgin;
		this.data.startAngle = startAngle;
		Vector3 vector = MathfX.RotateVector(Vector3.up, this.data.startAngle);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(startPoint1, vector, this.data.laserLength, Laser._effectiveLayer);
		this.data.point1 = startPoint1;
		this.data.point2 = raycastHit2D.point;
		if (!raycastHit2D)
		{
			this.data.point2 = this.data.point1 + vector * this.data.laserLength;
		}
		if (this.data.canHitGround)
		{
			base.transform.GetChild(0).position = this.data.point2;
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		base.StartCoroutine(this.UseWithoutRotation());
	}

	public void AppearLaser(Vector3 dir, GameObject orgin, bool oneShot = false)
	{
		EventManager.RegisterEvent<EventArgs>("DestroyEffect", new EventManager.FBEventHandler<EventArgs>(this.DestroyEffect), EventManager.ListenerQueue.Game);
		this.orgin = orgin;
		this.data.point1 = base.transform.position;
		this.hit = Physics2D.Raycast(this.data.point1, dir, this.data.laserLength, Laser._effectiveLayer);
		this.data.point2 = this.hit.point;
		if (!this.hit)
		{
			this.data.point2 = this.data.point1 + dir * this.data.laserLength;
		}
		if (this.data.canHitGround)
		{
			base.transform.GetChild(0).position = this.data.point2;
			base.transform.GetChild(0).gameObject.SetActive(true);
		}
		if (oneShot)
		{
			base.StartCoroutine(this.UseWithoutRotation());
		}
	}

	public IEnumerator UseWithoutRotation()
	{
		this.line.startWidth = 0f;
		this.line.endWidth = 0f;
		this.ShowLine();
		for (float i = 0f; i < this.data.appearTime; i += Time.deltaTime)
		{
			this.widthTemp = Mathf.Lerp(0f, this.data.laserWidth, i / this.data.appearTime);
			this.widthTemp = ((this.widthTemp >= 0.05f) ? this.widthTemp : 0f);
			this.line.startWidth = this.widthTemp;
			this.line.endWidth = this.widthTemp;
			yield return null;
		}
		this.line.startWidth = this.data.laserWidth;
		this.line.endWidth = this.data.laserWidth;
		yield return new WaitForSeconds(0.1f);
		for (float j = 0f; j < this.data.appearTime; j += Time.deltaTime)
		{
			this.widthTemp = Mathf.Lerp(this.data.laserWidth, 0f, j / this.data.appearTime);
			this.widthTemp = ((this.widthTemp >= 0.05f) ? this.widthTemp : 0f);
			this.line.startWidth = this.widthTemp;
			this.line.endWidth = this.widthTemp;
			yield return null;
		}
		base.transform.GetChild(0).gameObject.SetActive(false);
		this.line.startWidth = 0f;
		this.line.endWidth = 0f;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	public void SetAtkData(JsonData data, int damage)
	{
		this.atkData = data;
		this.damage = damage;
	}

	public bool DestroyEffect(string eventName, object sender, EventArgs msg)
	{
		if ((GameObject)sender == this.orgin)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		return true;
	}

	private void OnDestroy()
	{
		EventManager.UnregisterEvent<EventArgs>("DestroyEffect", new EventManager.FBEventHandler<EventArgs>(this.DestroyEffect), EventManager.ListenerQueue.Game);
	}

	private int currentShaderIndex;

	private bool canRun;

	private float startTime;

	private float widthTemp;

	private float angleTemp;

	private float firstHitTime;

	private Vector3 startPoint2;

	private RaycastHit2D hit;

	public LineRenderer line;

	[SerializeField]
	public LaserData data;

	private GameObject _orgin;

	private JsonData atkData;

	private int damage;

	private static int _effectiveLayer;

	private float _interval;

	private bool _hit;

	private EnemyAttribute _enemyAttribute;
}
