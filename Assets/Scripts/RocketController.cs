using System;
using UnityEngine;

public class RocketController : BaseBehaviour
{
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	private void Start()
	{
		this.t = base.transform;
		this.GetEffectSettingsComponent(this.t);
		if (this.effectSettings == null)
		{
			UnityEngine.Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		if (!this.IsRootMove)
		{
			this.startParentPosition = base.transform.parent.position;
		}
		if (this.GoLight != null)
		{
			this.tLight = this.GoLight.transform;
		}
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	private void OnDisable()
	{
		if (this.ResetParentPositionOnDisable && this.isInitializedOnStart && !this.IsRootMove)
		{
			base.transform.parent.position = this.startParentPosition;
		}
	}

	private void InitializeDefault()
	{
		this.hit = default(RaycastHit);
		this.onCollision = false;
		this.smootRandomPos = default(Vector3);
		this.oldSmootRandomPos = default(Vector3);
		this.deltaSpeed = 0f;
		this.startTime = 0f;
		this.randomSpeed = 0f;
		this.randomRadiusX = 0f;
		this.randomRadiusY = 0f;
		this.randomDirection1 = 0;
		this.randomDirection2 = 0;
		this.randomDirection3 = 0;
		this.frameDroped = false;
		this.tRoot = ((!this.IsRootMove) ? base.transform.parent : this.effectSettings.transform);
		this.startPosition = this.tRoot.position;
		if (this.effectSettings.Target != null)
		{
			this.tTarget = this.effectSettings.Target.transform;
		}
		else if (!this.effectSettings.UseMoveVector)
		{
			UnityEngine.Debug.Log("You must setup the the target or the motion vector");
		}
		if ((double)this.effectSettings.EffectRadius > 0.001)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.effectSettings.EffectRadius;
			this.randomTargetOffsetXZVector = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			this.randomTargetOffsetXZVector = Vector3.zero;
		}
		if (!this.effectSettings.UseMoveVector)
		{
			this.forwardDirection = this.tRoot.position + (this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position).normalized * this.effectSettings.MoveDistance;
			this.GetTargetHit();
		}
		else
		{
			this.forwardDirection = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
		}
		if (this.IsLookAt)
		{
			if (!this.effectSettings.UseMoveVector)
			{
				this.tRoot.LookAt(this.tTarget);
			}
			else
			{
				this.tRoot.LookAt(this.forwardDirection);
			}
		}
		this.InitRandomVariables();
	}

	private void Update()
	{
		if (!this.frameDroped)
		{
			this.frameDroped = true;
			return;
		}
		if (((!this.effectSettings.UseMoveVector && this.tTarget == null) || this.onCollision) && this.frameDroped)
		{
			return;
		}
		Vector3 vector;
		if (!this.effectSettings.UseMoveVector)
		{
			vector = ((!this.effectSettings.IsHomingMove) ? this.forwardDirection : this.tTarget.position);
		}
		else
		{
			vector = this.forwardDirection;
		}
		float num = Vector3.Distance(this.tRoot.position, vector);
		float num2 = this.effectSettings.MoveSpeed * Time.deltaTime;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num <= this.effectSettings.ColliderRadius)
		{
			this.hit = default(RaycastHit);
			this.CollisionEnter();
		}
		Vector3 normalized = (vector - this.tRoot.position).normalized;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.tRoot.position, normalized, out raycastHit, num2 + this.effectSettings.ColliderRadius, this.effectSettings.LayerMask))
		{
			this.hit = raycastHit;
			vector = raycastHit.point - normalized * this.effectSettings.ColliderRadius;
			this.CollisionEnter();
		}
		if (this.IsCenterLightPosition && this.GoLight != null)
		{
			this.tLight.position = (this.startPosition + this.tRoot.position) / 2f;
		}
		Vector3 b = default(Vector3);
		if (this.RandomMoveCoordinates != RandomMoveCoordinates.None)
		{
			this.UpdateSmootRandomhPos();
			b = this.smootRandomPos - this.oldSmootRandomPos;
		}
		float num3 = 1f;
		if (this.Acceleration.length > 0)
		{
			float time = (Time.time - this.startTime) / this.AcceleraionTime;
			num3 = this.Acceleration.Evaluate(time);
		}
		Vector3 vector2 = Vector3.MoveTowards(this.tRoot.position, vector, this.effectSettings.MoveSpeed * Time.deltaTime * num3);
		Vector3 vector3 = vector2 + b;
		if (this.IsLookAt && this.effectSettings.IsHomingMove)
		{
			this.tRoot.LookAt(vector3);
		}
		if (this.IsLocalSpaceRandomMove && this.IsRootMove)
		{
			this.tRoot.position = vector2;
			this.t.localPosition += b;
		}
		else
		{
			this.tRoot.position = vector3;
		}
		this.oldSmootRandomPos = this.smootRandomPos;
	}

	private void CollisionEnter()
	{
		if (this.EffectOnHitObject != null && this.hit.transform != null)
		{
			Transform transform = this.hit.transform;
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EffectOnHitObject);
			gameObject.transform.parent = componentInChildren.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.GetComponent<AddMaterialOnHit>().UpdateMaterial(this.hit);
		}
		if (this.AttachAfterCollision)
		{
			this.tRoot.parent = this.hit.transform;
		}
		if (this.SendCollisionMessage)
		{
			CollisionInfo e = new CollisionInfo
			{
				Hit = this.hit
			};
			this.effectSettings.OnCollisionHandler(e);
			if (this.hit.transform != null)
			{
				ShieldCollisionBehaviour component = this.hit.transform.GetComponent<ShieldCollisionBehaviour>();
				if (component != null)
				{
					component.ShieldCollisionEnter(e);
				}
			}
		}
		this.onCollision = true;
	}

	private void InitRandomVariables()
	{
		this.deltaSpeed = this.RandomMoveSpeed * UnityEngine.Random.Range(1f, 1000f * this.RandomRange + 1f) / 1000f - 1f;
		this.startTime = Time.time;
		this.randomRadiusX = UnityEngine.Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomRadiusY = UnityEngine.Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomSpeed = UnityEngine.Random.Range(this.RandomMoveSpeed / 20f, this.RandomMoveSpeed * 100f) / 100f;
		this.randomDirection1 = ((UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1);
		this.randomDirection2 = ((UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1);
		this.randomDirection3 = ((UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1);
	}

	private void GetTargetHit()
	{
		Ray ray = new Ray(this.tRoot.position, Vector3.Normalize(this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position));
		Collider componentInChildren = this.tTarget.GetComponentInChildren<Collider>();
		RaycastHit raycastHit;
		if (componentInChildren != null && componentInChildren.Raycast(ray, out raycastHit, this.effectSettings.MoveDistance))
		{
			this.hit = raycastHit;
		}
	}

	private void UpdateSmootRandomhPos()
	{
		float num = Time.time - this.startTime;
		float num2 = num * this.randomSpeed;
		float f = num * this.deltaSpeed;
		float num4;
		float num5;
		if (this.IsDeviation)
		{
			float num3 = Vector3.Distance(this.tRoot.position, this.hit.point) / this.effectSettings.MoveDistance;
			num4 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX * num3;
			num5 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.14159274f / 2f * num + Mathf.Sin(f)) * this.randomRadiusY * num3;
		}
		else
		{
			num4 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX;
			num5 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.14159274f / 2f * num + Mathf.Sin(f)) * this.randomRadiusY;
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XY)
		{
			this.smootRandomPos = new Vector3(num4, num5, 0f);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XZ)
		{
			this.smootRandomPos = new Vector3(num4, 0f, num5);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.YZ)
		{
			this.smootRandomPos = new Vector3(0f, num4, num5);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XYZ)
		{
			this.smootRandomPos = new Vector3(num4, num5, (num4 + num5) / 2f * (float)this.randomDirection1);
		}
	}

	public float RandomMoveRadius;

	public float RandomMoveSpeed;

	public float RandomRange;

	public RandomMoveCoordinates RandomMoveCoordinates;

	public GameObject EffectOnHitObject;

	public GameObject GoLight;

	public AnimationCurve Acceleration;

	public float AcceleraionTime = 1f;

	public bool IsCenterLightPosition;

	public bool IsLookAt;

	public bool AttachAfterCollision;

	public bool IsRootMove = true;

	public bool IsLocalSpaceRandomMove;

	public bool IsDeviation;

	public bool SendCollisionMessage = true;

	public bool ResetParentPositionOnDisable;

	private EffectSettings effectSettings;

	private Transform tRoot;

	private Transform tTarget;

	private Transform t;

	private Transform tLight;

	private Vector3 forwardDirection;

	private Vector3 startPosition;

	private Vector3 startParentPosition;

	private RaycastHit hit;

	private Vector3 smootRandomPos;

	private Vector3 oldSmootRandomPos;

	private float deltaSpeed;

	private float startTime;

	private float randomSpeed;

	private float randomRadiusX;

	private float randomRadiusY;

	private int randomDirection1;

	private int randomDirection2;

	private int randomDirection3;

	private bool onCollision;

	private bool isInitializedOnStart;

	private Vector3 randomTargetOffsetXZVector;

	private bool frameDroped;
}
