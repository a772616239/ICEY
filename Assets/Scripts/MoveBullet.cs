using System;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.newParticle != null && Time.timeSinceLevelLoad > 3f)
		{
			this.AddNewParticle();
			this.newParticle = null;
		}
		if (this.local)
		{
			base.transform.Translate(new Vector3(this.translationSpeedX, this.translationSpeedY, this.translationSpeedZ) * Time.deltaTime);
		}
		if (!this.local)
		{
			base.transform.Translate(new Vector3(this.translationSpeedX, this.translationSpeedY, this.translationSpeedZ) * Time.deltaTime, Space.World);
		}
	}

	private void AddNewParticle()
	{
		UnityEngine.Object.Destroy(base.gameObject.GetComponentInChildren<ParticleSystem>());
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.newParticle, base.transform.position, base.transform.rotation), this.deadTime);
		UnityEngine.Object.Destroy(base.gameObject, this.deadTime);
	}

	public float translationSpeedX;

	public float translationSpeedY = 1f;

	public float translationSpeedZ;

	public float deadTime = 4f;

	private bool local = true;

	public GameObject newParticle;
}
