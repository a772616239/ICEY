using System;
using UnityEngine;

public class FBParticleType : BaseBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	private void Awake()
	{
		this.getSpeed();
	}

	private void OnEnable()
	{
		this.initialX = base.transform.position.x;
		this.mNewObjectIsUsed = false;
		this.startTime = Time.time;
		this.getSpeed();
	}

	private void Start()
	{
		this.initialX = base.transform.position.x;
		this.mNewObjectIsUsed = false;
		this.startTime = Time.time;
		this.getSpeed();
	}

	private void Update()
	{
		if (this.distance == 0f || (this.distance != 0f && Mathf.Abs(base.transform.position.x - this.initialX) < this.distance))
		{
			if (this.useSpeed)
			{
				Vector3 a = this.mRandomSpeed + this.mConstantRandomSpeed;
				base.transform.Translate(a * Time.deltaTime);
				if (this.br != null)
				{
					this.br.StartCalculate((a.x <= 0f) ? -1 : 1);
				}
			}
			else
			{
				base.transform.Translate(this.mConstantRandomSpeed * Time.deltaTime);
				if (this.br != null)
				{
					this.br.StartCalculate((this.mConstantRandomSpeed.x <= 0f) ? -1 : 1);
				}
			}
			base.transform.Translate(0f, -this.gravity * this.deltaTime * Time.deltaTime, 0f);
		}
		if (this.playTime != 0f && this.playTime < this.deltaTime && !this.mNewObjectIsUsed && this.newObject != null)
		{
			this.mNewObjectIsUsed = true;
			for (int i = 0; i < this.newCount; i++)
			{
				this.GenerateObject(base.transform.position, Vector3.zero, i);
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag(this.colliderName) && this.playTime == 0f && this.canCollide(other.gameObject))
		{
			if (this.newObject != null)
			{
				Vector3 position = other.transform.parent.transform.position + other.GetComponentInParent<Pivot>().attackHurtEffectOffset;
				this.GenerateObject(position, new Vector3(0f, (float)((other.transform.localScale.x >= 0f) ? 180 : 0), 0f), 1);
			}
			base.Invoke("disableSelf", this.destroyTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(this.colliderName) && this.canCollide(other.gameObject))
		{
			if (this.newObject != null)
			{
				this.mNewObjectIsUsed = true;
				Vector3 position = other.transform.parent.transform.position + other.GetComponentInParent<Pivot>().attackHurtEffectOffset;
				this.GenerateObject(position, new Vector3(0f, (float)((other.transform.localScale.x >= 0f) ? 180 : 0), 0f), 1);
			}
			base.Invoke("disableSelf", this.destroyTime);
		}
	}

	public Vector3 GetRandomVector3()
	{
		float max = Mathf.Max(this.speed1.x, this.speed2.x);
		float min = Mathf.Min(this.speed1.x, this.speed2.x);
		float max2 = Mathf.Max(this.speed1.y, this.speed2.y);
		float min2 = Mathf.Min(this.speed1.y, this.speed2.y);
		float max3 = Mathf.Max(this.speed1.z, this.speed2.z);
		float min3 = Mathf.Min(this.speed1.z, this.speed2.z);
		return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min2, max2), UnityEngine.Random.Range(min3, max3));
	}

	private void disableSelf()
	{
		EffectController.TerminateEffect(base.gameObject);
	}

	private bool canCollide(GameObject other)
	{
		return (Vector3.Distance(other.transform.position, base.transform.position) < 3f || Mathf.Abs(other.transform.position.x - base.transform.position.x) < 3f) && !this.mNewObjectIsUsed;
	}

	private void PrintLog(GameObject other)
	{
	}

	public void getSpeed()
	{
		if (this.speedRangeMode == FBParticleType.speedRange.Double)
		{
			this.mRandomSpeed = this.GetRandomVector3();
		}
		else
		{
			this.mRandomSpeed = this.speed1;
		}
		this.mConstantRandomSpeed = new Vector3((UnityEngine.Random.Range(-1f, 1f) <= 0f) ? (-this.speed) : this.speed, (UnityEngine.Random.Range(-1f, 1f) <= 0f) ? (-this.speed) : this.speed, (UnityEngine.Random.Range(-1f, 1f) <= 0f) ? (-this.speed) : this.speed);
	}

	public void GenerateObject(Vector3 position, Vector3 rotation, int i)
	{
		GameObject gameObject = R.Effect.UsePool(this.newObject.name);
		if (gameObject == null)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.newObject, position, Quaternion.Euler(Vector3.zero));
			transform.name = transform.name.Replace("(Clone)", string.Empty);
		}
		else
		{
			gameObject.SetActive(true);
			gameObject.transform.parent = null;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.position = position;
			ChangeParticle component = gameObject.GetComponent<ChangeParticle>();
			if (component != null)
			{
				if (component.usePosition)
				{
					gameObject.transform.position = new Vector3(base.transform.position.x, component.positionY, base.transform.position.z);
				}
				if (component.useAverageSpeed)
				{
					float x = component.minSpeed + (component.maxSpeed - component.minSpeed) * (float)i / (float)this.newCount;
					FBParticleType component2 = gameObject.GetComponent<FBParticleType>();
					component2.speed1 = new Vector3(x, 0f, 0f);
					component2.getSpeed();
				}
			}
			gameObject.transform.rotation = Quaternion.Euler(rotation);
		}
	}

	public const float COLLISION_DISTANCE = 3f;

	private Vector3 mRandomSpeed;

	private Vector3 mConstantRandomSpeed;

	private bool mNewObjectIsUsed;

	private float startTime;

	private float initialX;

	private string colliderName = "EnemyHurtBox";

	private BulletRotation br;

	[SerializeField]
	public float playTime;

	[SerializeField]
	public bool useSpeed;

	[SerializeField]
	public FBParticleType.speedRange speedRangeMode;

	[SerializeField]
	private Transform newObject;

	[SerializeField]
	public float gravity;

	[SerializeField]
	public float distance;

	[SerializeField]
	public float speed;

	[SerializeField]
	public Vector3 speed1;

	[SerializeField]
	public Vector3 speed2;

	[SerializeField]
	public float destroyTime = 0.01f;

	[SerializeField]
	public int newCount = 1;

	public enum speedRange
	{
		Single,
		Double
	}
}
