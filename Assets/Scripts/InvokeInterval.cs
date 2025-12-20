using System;
using System.Collections.Generic;
using UnityEngine;

public class InvokeInterval : BaseBehaviour
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
		this.GetEffectSettingsComponent(base.transform);
		this.goInstances = new List<GameObject>();
		this.count = (int)(this.Duration / this.Interval);
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GO, base.transform.position, default(Quaternion));
			gameObject.transform.parent = base.transform;
			EffectSettings component = gameObject.GetComponent<EffectSettings>();
			component.Target = this.effectSettings.Target;
			component.IsHomingMove = this.effectSettings.IsHomingMove;
			component.MoveDistance = this.effectSettings.MoveDistance;
			component.MoveSpeed = this.effectSettings.MoveSpeed;
			component.CollisionEnter += delegate(object n, CollisionInfo e)
			{
				this.effectSettings.OnCollisionHandler(e);
			};
			component.ColliderRadius = this.effectSettings.ColliderRadius;
			component.EffectRadius = this.effectSettings.EffectRadius;
			component.EffectDeactivated += this.effectSettings_EffectDeactivated;
			this.goInstances.Add(gameObject);
			gameObject.SetActive(false);
		}
		this.InvokeAll();
		this.isInitialized = true;
	}

	private void InvokeAll()
	{
		for (int i = 0; i < this.count; i++)
		{
			base.Invoke("InvokeInstance", (float)i * this.Interval);
		}
	}

	private void InvokeInstance()
	{
		this.goInstances[this.goIndexActivate].SetActive(true);
		this.goInstances[this.goIndexActivate].GetComponentInChildren<EnemyBulletLaucher>().attacker = this.attacker;
		if (this.goIndexActivate >= this.goInstances.Count - 1)
		{
			this.goIndexActivate = 0;
		}
		else
		{
			this.goIndexActivate++;
		}
	}

	private void effectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		EffectSettings effectSettings = sender as EffectSettings;
		effectSettings.transform.position = base.transform.position;
		if (this.goIndexDeactivate >= this.count - 1)
		{
			this.effectSettings.Deactivate();
			this.goIndexDeactivate = 0;
		}
		else
		{
			this.goIndexDeactivate++;
		}
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InvokeAll();
		}
	}

	private void OnDisable()
	{
	}

	public GameObject GO;

	public float Interval = 0.3f;

	public float Duration = 3f;

	private List<GameObject> goInstances;

	private EffectSettings effectSettings;

	private int goIndexActivate;

	private int goIndexDeactivate;

	private bool isInitialized;

	private int count;

	public Transform attacker;
}
