using System;
using UnityEngine;

public class EffectGeneratorAnimEvent : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AnimEventGeneratorEffect(int id)
	{
		R.Effect.Generate(id, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
	}
}
