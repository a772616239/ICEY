using System;
using CIS;
using UnityEngine;

public class GlobalWindGenerator : SingletonMonoBehaviourClass<GlobalWindGenerator>, IBlowee
{
	public float maxPower
	{
		get
		{
			return this.initPower;
		}
	}

	public Vector2 GetDir(IBlowable blowable, Vector2 pos)
	{
		return this._dir;
	}

	public float GetPower(IBlowable blowable, Vector2 pos)
	{
		return this.power;
	}

	public float GetMaxAccelSqr()
	{
		return float.MaxValue;
	}

	public void ParserIntegrator(Integrator integrator)
	{
	}

	public Vector2 dir
	{
		get
		{
			return this._dir;
		}
	}

	public GameObject GetGO()
	{
		return base.gameObject;
	}

	private void Awake()
	{
		this.initPower = this.power;
		this.initDir = this.windDir.normalized;
		this.alterInitDir = this.alterWindDir.normalized;
		this.initWindAccel = this.windAccel;
		this.power = 0f;
		this.generateP = this.wavePerSecond / 60f;
		this.state = GlobalWindGenerator.State.Generating;
	}

	private void Update()
	{
		if (this.state == GlobalWindGenerator.State.Generating)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			if (num < this.generateP)
			{
				this.power = 0.01f;
				this.state = GlobalWindGenerator.State.Accel;
				this.windAccel = UnityEngine.Random.Range(0.5f * this.initWindAccel, 2f * this.initWindAccel);
				Vector2 vector = new Vector2(UnityEngine.Random.Range(this.initDir.x, this.alterInitDir.x), UnityEngine.Random.Range(this.alterInitDir.y, this.initDir.y));
				this.alterWindDir = vector.normalized;
			}
		}
		else if (this.state == GlobalWindGenerator.State.Accel)
		{
			if (this.power < this.initPower)
			{
				this.power += this.windAccel * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			}
			else
			{
				this.state = GlobalWindGenerator.State.Waving;
				this.waveTime = UnityEngine.Random.Range(6f, 12f);
			}
		}
		else if (this.state == GlobalWindGenerator.State.Waving)
		{
			this.waveTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime * this.waveSpeed;
			if (this.waveTimer < this.waveTime * this.waveSpeed)
			{
				float num2 = (1f + Mathf.Cos(this.waveTimer)) / 2f;
				this.power = this.initPower / 6f + 5f * this.initPower / 6f * num2;
				this._dir = Vector2.Lerp(this.initDir, this.alterWindDir, num2).normalized;
			}
			else
			{
				this.waveTimer = 0f;
				this.state = GlobalWindGenerator.State.Fall;
				this.fallTime = UnityEngine.Random.Range(0.5f, 1f);
				this.initFallPower = this.power;
				this.initFallDir = this._dir;
			}
		}
		else if (this.state == GlobalWindGenerator.State.Fall)
		{
			this.fallTimer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			if (this.fallTimer < this.fallTime)
			{
				float t = this.fallTimer / this.fallTime;
				this.power = Mathf.Lerp(this.initFallPower, 0f, t);
				this._dir = Vector2.Lerp(this.initFallDir, -Vector2.up, t).normalized;
			}
			else
			{
				this.fallTimer = 0f;
				this.power = 0f;
				this.state = GlobalWindGenerator.State.Generating;
			}
		}
	}

	private void OnGUI()
	{
		if (!this.debug)
		{
			return;
		}
		GUI.BeginGroup(new Rect(10f, 100f, 200f, 200f));
		GUILayout.Label("Win Dir: " + this._dir, new GUILayoutOption[0]);
		GUILayout.Label("Win power: " + this.power, new GUILayoutOption[0]);
		GUILayout.Label("Win state: " + this.state, new GUILayoutOption[0]);
		if (this._dir.x != 0f || this._dir.y != 0f)
		{
			Vector2 vector = new Vector2(50f, 100f);
			Vector2 dir = this._dir;
			dir.y *= -1f;
			GuiHelper.DrawLine(vector, vector + dir * 80f, Color.green, 1);
			dir = this.initDir;
			dir.y *= -1f;
			GuiHelper.DrawLine(vector, vector + dir * 80f, Color.red, 1);
			dir = this.alterWindDir;
			dir.y *= -1f;
			GuiHelper.DrawLine(vector, vector + dir * 80f, Color.red, 1);
		}
		GUI.EndGroup();
	}

	public Vector2 windDir;

	public Vector2 alterWindDir;

	public float power;

	public float wavePerSecond;

	public float windAccel;

	public float waveSpeed;

	private float generateP;

	private float initWindAccel;

	private float initPower;

	private Vector2 initDir;

	private Vector2 alterInitDir;

	private Vector2 _dir;

	private float waveTime;

	private float waveTimer;

	private float fallTimer;

	private float initFallPower;

	private Vector2 initFallDir;

	private float fallTime;

	private GlobalWindGenerator.State state;

	private float windGenerateCondition;

	public bool debug;

	public enum State
	{
		Generating,
		Accel,
		Waving,
		Fall
	}
}
