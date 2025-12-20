using System;
using UnityEngine;

public class ShieldCollisionBehaviour : MonoBehaviour
{
	public void ShieldCollisionEnter(CollisionInfo e)
	{
		if (e.Hit.transform != null)
		{
			if (this.IsWaterInstance)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ExplosionOnHit);
				Transform transform = gameObject.transform;
				transform.parent = base.transform;
				float num = base.transform.localScale.x * this.ScaleWave;
				transform.localScale = new Vector3(num, num, num);
				transform.localPosition = new Vector3(0f, 0.001f, 0f);
				transform.LookAt(e.Hit.point);
			}
			else
			{
				if (this.EffectOnHit != null)
				{
					if (!this.CreateMechInstanceOnHit)
					{
						Transform transform2 = e.Hit.transform;
						Renderer componentInChildren = transform2.GetComponentInChildren<Renderer>();
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.EffectOnHit);
						gameObject2.transform.parent = componentInChildren.transform;
						gameObject2.transform.localPosition = Vector3.zero;
						AddMaterialOnHit component = gameObject2.GetComponent<AddMaterialOnHit>();
						component.SetMaterialQueue(this.currentQueue);
						component.UpdateMaterial(e.Hit);
					}
					else
					{
						GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.EffectOnHit);
						Transform transform3 = gameObject3.transform;
						transform3.parent = base.GetComponent<Renderer>().transform;
						transform3.localPosition = Vector3.zero;
						transform3.localScale = base.transform.localScale * this.ScaleWave;
						transform3.LookAt(e.Hit.point);
						transform3.Rotate(this.AngleFix);
						gameObject3.GetComponent<Renderer>().material.renderQueue = this.currentQueue - 1000;
					}
				}
				if (this.currentQueue > 4000)
				{
					this.currentQueue = 3001;
				}
				else
				{
					this.currentQueue++;
				}
				if (this.ExplosionOnHit != null)
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.ExplosionOnHit, e.Hit.point, default(Quaternion));
					gameObject4.transform.parent = base.transform;
				}
			}
		}
	}

	private void Update()
	{
	}

	public GameObject EffectOnHit;

	public GameObject ExplosionOnHit;

	public bool IsWaterInstance;

	public float ScaleWave = 0.89f;

	public bool CreateMechInstanceOnHit;

	public Vector3 AngleFix;

	public int currentQueue = 3001;
}
