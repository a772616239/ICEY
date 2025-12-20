using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Cliff : MonoBehaviour
{
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Cliff.RebornPoint = base.transform;
		}
	}

	public static void Reset()
	{
		Cliff.RebornPoint = null;
	}

	public static Transform RebornPoint;
}
