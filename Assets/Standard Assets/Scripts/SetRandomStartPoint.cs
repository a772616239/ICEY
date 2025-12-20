using System;
using UnityEngine;

public class SetRandomStartPoint : MonoBehaviour
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
		if (this.effectSettings == null)
		{
			UnityEngine.Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	private void InitDefaultVariables()
	{
		if (base.GetComponent<ParticleSystem>() != null)
		{
			base.GetComponent<ParticleSystem>().Stop();
		}
		Vector3 position = this.effectSettings.Target.transform.position;
		Vector3 vector = new Vector3(position.x, this.Height, position.z);
		float num = UnityEngine.Random.Range(0f, this.RandomRange.x * 200f) / 100f - this.RandomRange.x;
		float num2 = UnityEngine.Random.Range(0f, this.RandomRange.y * 200f) / 100f - this.RandomRange.y;
		float num3 = UnityEngine.Random.Range(0f, this.RandomRange.z * 200f) / 100f - this.RandomRange.z;
		Vector3 position2 = new Vector3(vector.x + num, vector.y + num2, vector.z + num3);
		if (this.StartPointGo == null)
		{
			this.tRoot.position = position2;
		}
		else
		{
			this.StartPointGo.transform.position = position2;
		}
		if (base.GetComponent<ParticleSystem>() != null)
		{
			base.GetComponent<ParticleSystem>().Play();
		}
	}

	private void Update()
	{
	}

	public Vector3 RandomRange;

	public GameObject StartPointGo;

	public float Height = 10f;

	private EffectSettings effectSettings;

	private bool isInitialized;

	private Transform tRoot;
}
