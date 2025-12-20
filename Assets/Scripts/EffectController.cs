using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : SingletonMono<EffectController>
{
	public List<EffectAttr> fxSerializedData
	{
		get
		{
			return this._fxSerializedData;
		}
		set
		{
			this._fxSerializedData = value;
			this._fxData = null;
		}
	}

	public Dictionary<int, EffectAttr> fxData
	{
		get
		{
			if (this._fxData == null)
			{
				this._fxData = new Dictionary<int, EffectAttr>();
				for (int i = 0; i < this._fxSerializedData.Count; i++)
				{
					EffectAttr effectAttr = this._fxSerializedData[i];
					this._fxData.Add(effectAttr.id, effectAttr);
				}
			}
			return this._fxData;
		}
	}

	private void Start()
	{
		if (EffectController.AllowPreload)
		{
			this.Preload();
		}
	}

	public Transform Generate(int effectId, Transform target = null, Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), Vector3 scale = default(Vector3), bool useFxZNum = true)
	{
		GameObject gameObject = this.UsePool(this.fxData[effectId].effect.name);
		Transform transform = (!(gameObject != null)) ? UnityEngine.Object.Instantiate<Transform>(this.fxData[effectId].effect) : gameObject.transform;
		if (target != null)
		{
			transform.parent = target;
			transform.localPosition = position;
			transform.localRotation = Quaternion.Euler(Vector3.zero);
			if (!this.fxData[effectId].isFollow)
			{
				transform.parent = null;
				transform.position = target.position + position;
			}
		}
		else
		{
			transform.parent = null;
			transform.position = position;
		}
		switch (this.fxData[effectId].rotation)
		{
		case EffectController.FXRotationCondition.HaveEnemyDirectionY:
			transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, (float)((target.localScale.x <= 0f) ? 0 : 180), rotation.z));
			break;
		case EffectController.FXRotationCondition.RandomRotationZ:
			transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360)));
			break;
		case EffectController.FXRotationCondition.Preference:
			transform.localRotation = Quaternion.Euler(rotation);
			break;
		case EffectController.FXRotationCondition.HaveEnemyDirectionZ:
			transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, (float)((target.localScale.x <= 0f) ? 0 : 180)));
			break;
		case EffectController.FXRotationCondition.FollowPlayer:
			transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, (float)((target.localScale.x >= 0f) ? 0 : 180), rotation.z));
			break;
		case EffectController.FXRotationCondition.FollowTargeRotation:
			transform.localRotation = target.rotation;
			break;
		}
		transform.localScale = ((!(scale != Vector3.zero)) ? this.fxData[effectId].scale : scale);
		if (useFxZNum)
		{
			Vector3 position2 = transform.position;
			position2.z = LayerManager.ZNum.Fx;
			transform.position = position2;
		}
		transform.gameObject.SetActive(true);
		return transform;
	}

	public GameObject UsePool(string effectName)
	{
		if (this._objectPoolDict.ContainsKey(effectName))
		{
			return this._objectPoolDict[effectName].GetObject();
		}
		return null;
	}

	public static void TerminateEffect(GameObject target)
	{
		AutoDisableEffect component = target.GetComponent<AutoDisableEffect>();
		if (component)
		{
			component.Disable(target.transform);
		}
		else
		{
			UnityEngine.Object.Destroy(target);
		}
	}

	private void Preload()
	{
		this._objectPoolDict = PoolController.EffectDict;
		for (int i = 0; i < this.fxSerializedData.Count; i++)
		{
			try
			{
				EffectAttr effectAttr = this.fxSerializedData[i];
				if (effectAttr.effect != null)
				{
					AutoDisableEffect component = effectAttr.effect.GetComponent<AutoDisableEffect>();
					if (!this._objectPoolDict.ContainsKey(effectAttr.effect.name) && component != null)
					{
						ObjectPool objectPool = new ObjectPool();
						base.StartCoroutine(objectPool.Init(effectAttr.effect.gameObject, base.gameObject, effectAttr.effectStartCount, effectAttr.maxCount));
						this._objectPoolDict.Add(effectAttr.effect.name, objectPool);
					}
				}
			}
			catch (Exception)
			{
				Log.Error(i);
			}
		}
	}

	[SerializeField]
	private List<EffectAttr> _fxSerializedData;

	private Dictionary<int, EffectAttr> _fxData;

	private Dictionary<string, ObjectPool> _objectPoolDict;

	public static bool AllowPreload = true;

	public enum FXRotationCondition
	{
		HaveEnemyDirectionY = 1,
		RandomRotationZ,
		Preference,
		HaveEnemyDirectionZ,
		FollowPlayer,
		FollowTargeRotation
	}
}
