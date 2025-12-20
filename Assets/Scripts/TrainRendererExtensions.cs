using System;
using System.Collections;
using UnityEngine;

public static class TrainRendererExtensions
{
	public static void Reset(this TrailRenderer trail, MonoBehaviour instance)
	{
		instance.StartCoroutine(TrainRendererExtensions.ResetTrail(trail));
	}

	private static IEnumerator ResetTrail(TrailRenderer trail)
	{
		float trailTime = trail.time;
		trail.time = 0f;
		yield return null;
		trail.time = trailTime;
		yield break;
	}
}
