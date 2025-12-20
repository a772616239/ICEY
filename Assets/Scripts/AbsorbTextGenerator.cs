using System;
using System.Collections;
using UnityEngine;

public class AbsorbTextGenerator : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public IEnumerator Generate()
	{
		for (int i = 0; i < this.maxParticle; i++)
		{
			R.Effect.Generate(this.firstId + UnityEngine.Random.Range(0, this.textParticle.Length), base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			yield return new WaitForSeconds(this.timeSpan);
		}
		yield break;
	}

	[SerializeField]
	private Transform[] textParticle;

	[SerializeField]
	private int firstId;

	[SerializeField]
	private int maxParticle;

	[SerializeField]
	private float timeSpan;
}
